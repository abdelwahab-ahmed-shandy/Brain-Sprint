using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;
using System.Threading;

using static Models.ViewModels.ContentManagementCreateVM;

namespace BrainSprint.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ("SuperAdmin,Admin"))]
    public class CoursesController : Controller
    {
        private readonly ICourseRepository _course;
        private readonly ILogger<CoursesController> _logger;
        private readonly IInstructorRepository _instructor;
        private readonly ILearningPathRepository _learningPath;
        private readonly ICourseLearningPathRepository _courseLearningPath;
        public CoursesController(ICourseLearningPathRepository courseLearningPathRepository, ICourseRepository courseRepository, ILogger<CoursesController> logger, IInstructorRepository instructor, ILearningPathRepository learningPath)
        {
            _course = courseRepository;
            _logger = logger;
            _instructor = instructor;
            _learningPath = learningPath;
            _courseLearningPath = courseLearningPathRepository;
        }


        #region View All Cources : 

        public async Task<IActionResult> Index(string? query, string? status, int page = 1)
        {
            var coursesQuery = _course.Get(includes: [c => c.Instructor, c => c.CourseLearningPaths]).ToList();

            var courseList = coursesQuery.Select(c => new ContentManagementVM
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Price = c.Price,
                Discount = c.Discount,
                ImgUrl = c.ImgUrl,
                VideoUrl = c.VideoUrl,
                Duration = c.Duration,
                InstructorName = c.Instructor?.ApplicationUser?.FirstName + " " + c.Instructor?.ApplicationUser?.LastName,
                LearningPathName = c.CourseLearningPaths.FirstOrDefault()?.LearningPath.Name,
                CreatedBy = c.CreatedBy,
                CreatedDateUtc = c.CreatedDateUtc,
                UpdatedBy = c.UpdatedBy,
                UpdatedDateUtc = c.UpdatedDateUtc,
            });


            if (!string.IsNullOrEmpty(query))
            {
                courseList = courseList.Where(u =>
                    (u.Name?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (u.Description?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (u.ImgUrl?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (u.VideoUrl?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (u.InstructorName?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (u.LearningPathName?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (u.CreatedBy?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (u.UpdatedBy?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (u.UpdatedDateUtc?.ToString("yyyy-MM-dd").Contains(query) == true) ||
                    (u.CreatedDateUtc.ToString("yyyy-MM-dd").Contains(query) == true) ||
                    (u.Duration?.ToString().Contains(query) == true) ||
                    (u.Discount?.ToString().Contains(query) == true) ||
                     u.Price.ToString().Contains(query)
                ).ToList();
            }

            var Pagina = new PaginationVM
            {
                CurrentPage = page,
                TotalItems = courseList.Count(),
                PageSize = 5,
                Query = query,
                StatusFilter = status
            };

            var paginatedCourses = courseList
                .Skip((page - 1) * Pagina.PageSize)
                .Take(Pagina.PageSize)
                .ToList();

            return View(new CoursesVM
            {
                Courses = paginatedCourses,
                Pagination = Pagina
            });
        }

        #endregion



        #region Add Cource

        [HttpGet]
        public IActionResult Create()
        {
            var instructors = _instructor.Get(includes: [i => i.ApplicationUser]).ToList();

            var learningPaths = _learningPath.Get().ToList();

            ViewBag.Instructors = instructors;

            ViewBag.LearningPaths = learningPaths;

            return View(new Models.Course());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course, IFormFile imageFile, int learningPathId)
        {
            var instructor = _instructor.GetOne(i => i.Id == course.InstructorId);
            if (instructor == null)
            {
                ModelState.AddModelError("InstructorId", "Selected instructor does not exist.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle image upload
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Create unique filename
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Assets", "Admin", "courses");

                        // Create directory if it doesn't exist
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var filePath = Path.Combine(uploadsFolder, fileName);

                        // Save file to server
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        // Set image URL in the course object
                        course.ImgUrl = $"/Assets/Admin/courses/{fileName}";
                    }

                    // Add course to database
                    _course.Create(course);
                    _course.SaveDB();

                    // Associate with learning path if selected
                    if (learningPathId > 0)
                    {
                        var courseLearningPath = new CourseLearningPath
                        {
                            CourseId = course.Id,
                            LearningPathId = learningPathId
                        };
                        _courseLearningPath.Create(courseLearningPath);
                        _courseLearningPath.SaveDB(); // Fixed: Changed from _learningPath to _courseLearningPath
                    }

                    TempData["SuccessMessage"] = "Course created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the course: " + ex.Message);
                    _logger.LogError(ex, "Error creating course");
                }
            }

            // If we got this far, something failed; redisplay form with existing data
            ViewBag.Instructors = _instructor.Get()
                .Include(i => i.ApplicationUser)
                .ToList();
            ViewBag.LearningPaths = _learningPath.Get().ToList();

            return View(course);
        }

        #endregion









    }
}
