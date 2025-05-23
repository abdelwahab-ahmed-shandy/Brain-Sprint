using BrainSprint.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using static Models.ViewModels.ContentManagementCreateVM;
using static Models.ViewModels.ContentManagementEditVM;

namespace BrainSprint.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = "Instructor,Admin,SuperAdmin")]
    public class LearningPathController : Controller
    {
        private readonly ILearningPathRepository _learningPathRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IInstructorRepository _instructorRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseLearningPathRepository _courseLearningPathRepository;
        private readonly IAuditService _auditService;
        private readonly ILogger<LearningPathController> _logger;
        public LearningPathController(ILearningPathRepository learningPathRepository,
        UserManager<ApplicationUser> userManager,
        IInstructorRepository instructorRepository,
        ICourseRepository courseRepository,
        ICourseLearningPathRepository courseLearningPathRepository,
        ILogger<LearningPathController> logger,
        IAuditService auditService)
        {
            _learningPathRepository = learningPathRepository;
            _userManager = userManager;
            _instructorRepository = instructorRepository;
            _courseRepository = courseRepository;
            _courseLearningPathRepository = courseLearningPathRepository;
            _logger = logger;
            _auditService = auditService;
        }


        #region View Learning Paths

        [HttpGet]
        public async Task<IActionResult> Index(string? query, string? status, int page = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var currentInstructor = await _instructorRepository
                .GetOneAsync(i => i.ApplicationUserId == user.Id);

            if (currentInstructor == null) return NotFound();

            // Define the includes
            var includes = new List<Expression<Func<LearningPath, object>>>
                                    {
                                        lp => lp.CourseLearningPaths
                                    };

            // Define the thenIncludes as function that operates on IQueryable
            var thenIncludes = new List<Func<IQueryable<LearningPath>, IQueryable<LearningPath>>>
                                        {
                                            q => q.Include(lp => lp.CourseLearningPaths).ThenInclude(clp => clp.Course)
                                        };

            var allLearningPaths = await _learningPathRepository.GetAsync(
                filter: lp => lp.InstructorId == currentInstructor.Id,
                includes: includes,
                thenIncludes: thenIncludes,
                tracked: false
            );

            var learningPathVMs = allLearningPaths.Select(lp => new ContentManagementVM
            {
                Id = lp.Id,
                Name = lp.Name,
                Description = lp.Description,
                IconUrl = lp.IconUrl,
                CreatedBy = lp.CreatedBy,
                CreatedDateUtc = lp.CreatedDateUtc,
                UpdatedBy = lp.UpdatedBy,
                UpdatedDateUtc = lp.UpdatedDateUtc,
                IsPublished = lp.CourseLearningPaths.Any(clp => clp.Course.IsPublished.GetValueOrDefault()),
                CourseCount = lp.CourseLearningPaths.Count,
                InstructorName = $"{currentInstructor.ApplicationUser.FirstName} {currentInstructor.ApplicationUser.LastName}",
                Status = lp.IsPublished == true ? CourseStatus.Approved : CourseStatus.Approved,

            }).ToList();

            // Rest of your filtering and pagination logic...
            if (!string.IsNullOrEmpty(query))
            {
                learningPathVMs = learningPathVMs.Where(lp =>
                    (lp.Name?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (lp.Description?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (lp.InstructorName?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    lp.CreatedDateUtc.ToString("yyyy-MM-dd").Contains(query) ||
                    (lp.UpdatedDateUtc?.ToString("yyyy-MM-dd")?.Contains(query) == true) ||
                    lp.Status.ToString().Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    lp.CourseCount.ToString().Contains(query)
                ).ToList();
            }

            if (!string.IsNullOrEmpty(status) && Enum.TryParse<CourseStatus>(status, true, out var statusFilter))
            {
                learningPathVMs = learningPathVMs
                    .Where(lp => lp.Status == statusFilter)
                    .ToList();
            }

            var pagination = new PaginationVM
            {
                CurrentPage = page,
                PageSize = 5,
                TotalItems = learningPathVMs.Count,
                Query = query,
                StatusFilter = status
            };

            if (page > pagination.TotalPages && pagination.TotalPages > 0)
            {
                page = pagination.TotalPages;
                pagination.CurrentPage = page;
            }

            var paginatedLearningPaths = learningPathVMs
                .OrderBy(lp => lp.Name)
                .Skip((page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            return View(new LearningPathsVM
            {
                LearningPaths = paginatedLearningPaths,
                Pagination = pagination
            });
        }

        #endregion



        #region Create Learning Path

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new LearningPathsCreateVM());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LearningPathsCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Get the current user
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            // Get the Instructor associated with the user
            var currentInstructor = await _instructorRepository
            .GetOneAsync(i => i.ApplicationUserId == currentUser.Id);

            if (currentInstructor == null)
            {
                // You can either create a new Instructor or return an error
                return NotFound("Instructor profile not found");
            }

            // Create a new Learning Path and associate it with the user
            var newLearningPath = new LearningPath
            {
                Name = model.Name,
                Description = model.Description,
                IconUrl = model.IconUrl,
                InstructorId = currentInstructor.Id,
                CreatedBy = User.Identity.Name,
                CreatedDateUtc = DateTime.UtcNow,
            };

            await _learningPathRepository.CreateAsync(newLearningPath);

            // Add a log to the activity 
            await _auditService.LogActivityAsync(
            currentUser.Id,
            "LearningPath.Created",
            $"Created new learning path: {model.Name}",
            newLearningPath.Id.ToString());

            TempData["notification"] = "Learning Path Created successfully!";
            TempData["MessageType"] = "success";

            return RedirectToAction(nameof(Index));
        }

        #endregion



        #region Edit Learning Path

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var learningPath = await _learningPathRepository.GetOneAsync(
                lp => lp.Id == id,
                includes: new List<Expression<Func<LearningPath, object>>>
                {
                    lp => lp.Instructor
                });

            if (learningPath == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || learningPath.Instructor?.ApplicationUserId != currentUser.Id)
            {
                return Forbid();
            }

            var model = new LearningPathEditVM
            {
                Id = learningPath.Id,
                Name = learningPath.Name,
                Description = learningPath.Description,
                IconUrl = learningPath.IconUrl
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LearningPathEditVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var learningPath = await _learningPathRepository.GetOneAsync(
                lp => lp.Id == model.Id,
                includes: new List<Expression<Func<LearningPath, object>>>
                {
            lp => lp.Instructor
                });

            if (learningPath == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || learningPath.Instructor?.ApplicationUserId != currentUser.Id)
            {
                return Forbid();
            }

            learningPath.Name = model.Name;
            learningPath.Description = model.Description;
            learningPath.IconUrl = model.IconUrl;
            learningPath.UpdatedBy = User.Identity.Name;
            learningPath.UpdatedDateUtc = DateTime.UtcNow;



            await _learningPathRepository.EditAsync(learningPath);
            await _learningPathRepository.SaveInDataBaseAsync();

            // Audit log
            await _auditService.LogActivityAsync(
                currentUser.Id,
                "LearningPath.Updated",
                $"Updated learning path: {model.Name}",
                learningPath.Id.ToString());


            TempData["notification"] = "Learning Path Updated successfully!";
            TempData["MessageType"] = "success";
            return RedirectToAction(nameof(Index));
        }

        #endregion



        #region Details Learning Path

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var learningPath = await _learningPathRepository.GetOneAsync(
                lp => lp.Id == id,
                includes: new List<Expression<Func<LearningPath, object>>>
                {
            lp => lp.CourseLearningPaths,
            lp => lp.Instructor
                },
                thenIncludes: new List<Func<IQueryable<LearningPath>, IQueryable<LearningPath>>>
                {
            q => q.Include(lp => lp.CourseLearningPaths)
                 .ThenInclude(clp => clp.Course)
                });

            if (learningPath == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || learningPath.Instructor?.ApplicationUserId != currentUser.Id)
            {
                return Forbid();
            }

            var model = new ContentManagementVM
            {
                Id = learningPath.Id,
                Name = learningPath.Name,
                Description = learningPath.Description,
                IconUrl = learningPath.IconUrl,
                CreatedBy = learningPath.Instructor?.ApplicationUser?.UserName ?? "System",
                CreatedDateUtc = learningPath.CreatedDateUtc,
                UpdatedBy = learningPath.UpdatedBy,
                UpdatedDateUtc = learningPath.UpdatedDateUtc,
                Courses = learningPath.CourseLearningPaths.Select(clp => new CourseDetailsVM
                {
                    CourseId = clp.CourseId,
                    CourseName = clp.Course.Title,
                    IsPublished = clp.Course.IsPublished.GetValueOrDefault(),
                    Status = clp.Course.Status,
                    PublishDate = clp.Course.CreatedDateUtc,
                    LastUpdated = clp.Course.UpdatedDateUtc,
                    ThumbnailUrl = clp.Course.ImgUrl
                }).ToList()
            };

            return View(model);
        }

        #endregion



        #region Delete Learning Path

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var learningPath = await _learningPathRepository.GetOneAsync(
                lp => lp.Id == id,
                includes: new List<Expression<Func<LearningPath, object>>>
                {
            lp => lp.Instructor,
            lp => lp.CourseLearningPaths
                });

            if (learningPath == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || learningPath.Instructor?.ApplicationUserId != currentUser.Id)
            {
                return Forbid();
            }

            return View(new LearningPathDeleteVM
            {
                Id = learningPath.Id,
                Name = learningPath.Name,
                CourseCount = learningPath.CourseLearningPaths.Count,
                CreatedDate = learningPath.CreatedDateUtc.ToLocalTime().ToString("d")
            });
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var learningPath = await _learningPathRepository.GetOneAsync(
                lp => lp.Id == id,
                includes: new List<Expression<Func<LearningPath, object>>>
                {
            lp => lp.Instructor,
            lp => lp.CourseLearningPaths
                });

            if (learningPath == null)
            {
                TempData["Notification"] = JsonConvert.SerializeObject(new
                {
                    Type = "error",
                    Message = "Learning Path not found!"
                });
                return RedirectToAction(nameof(Index));
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || learningPath.Instructor?.ApplicationUserId != currentUser.Id)
            {
                TempData["notification"] = "You are not authorized to delete this learning path!";
                TempData["MessageType"] = "Error";

                return RedirectToAction(nameof(Index));
            }

            try
            {

                // Remove course associations
                if (learningPath.CourseLearningPaths.Any())
                {
                    await _courseLearningPathRepository.DeleteAllAsync(learningPath.CourseLearningPaths);
                    await _courseLearningPathRepository.SaveInDataBaseAsync();
                }

                // Delete learning path

                await _learningPathRepository.DeleteAsync(learningPath);
                await _learningPathRepository.SaveInDataBaseAsync();

                // Audit log
                await _auditService.LogActivityAsync(
                    currentUser.Id,
                    "LearningPath.Deleted",
                    $"Deleted learning path: {learningPath.Name}",
                    learningPath.Id.ToString());

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting learning path {LearningPathId}", id);

                TempData["notification"] = "An error occurred while deleting the learning path!";
                TempData["MessageType"] = "Error";
            }

            TempData["notification"] = "Learning Path Is Deleted Successfully!";
            TempData["MessageType"] = "Success";

            return RedirectToAction(nameof(Index));
        }

        #endregion




    }
}
