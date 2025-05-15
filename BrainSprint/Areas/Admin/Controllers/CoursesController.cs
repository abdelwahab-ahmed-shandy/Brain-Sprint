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


        #region View All Courses

        //todo:here :
        public async Task<IActionResult> Index(string? query, CourseStatus? status = null, int page = 1)
        {
            var coursesQuery = _course.Get(includes: [
                                                        c => c.Instructor,
                                                        c => c.Instructor.ApplicationUser,
                                                        c => c.CourseLearningPaths
                                                    ]);


            coursesQuery = coursesQuery.Include(c => c.CourseLearningPaths)
                                       .ThenInclude(clp => clp.LearningPath);


            if (status.HasValue)
            {
                coursesQuery = coursesQuery.Where(c => c.Status == status.Value);
            }

            var courseList = coursesQuery.Select(c => new ContentManagementVM
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Price = c.Price,
                Discount = c.Discount,
                Duration = c.Duration,
                VideoUrl = c.VideoUrl,
                ImgUrl = c.ImgUrl,
                IsPublished = c.IsPublished,
                Status = c.Status,
                RejectionReason = c.RejectionReason,
                ReviewedDate = c.ReviewedDate,
                ReviewedBy = c.ReviewedBy,

                InstructorName = c.Instructor == null ? "Null" :
                    (c.Instructor.ApplicationUser == null ? "Null" :
                    $"{c.Instructor.ApplicationUser.FirstName ?? string.Empty} {c.Instructor.ApplicationUser.LastName ?? string.Empty}".Trim()),

                LearningPathName = c.CourseLearningPaths.FirstOrDefault() == null ? "Null" :
                      (c.CourseLearningPaths.First().LearningPath == null ? "Null" :
                      c.CourseLearningPaths.First().LearningPath.Name ?? "Null"),

                CreatedBy = c.CreatedBy,
                CreatedDateUtc = c.CreatedDateUtc,
                UpdatedBy = c.UpdatedBy,
                UpdatedDateUtc = c.UpdatedDateUtc
            }).ToList();

            if (!string.IsNullOrEmpty(query))
            {
                courseList = courseList.Where(c =>
                    (c.Title?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (c.Description?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (c.InstructorName?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (c.LearningPathName?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (c.CreatedBy?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (c.UpdatedBy?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (c.CreatedDateUtc.ToString("yyyy-MM-dd").Contains(query)) ||
                    (c.UpdatedDateUtc?.ToString("yyyy-MM-dd").Contains(query) == true) ||
                    (c.Duration.ToString().Contains(query)) ||
                    (c.Discount?.ToString().Contains(query) == true) ||
                    c.Price.ToString().Contains(query)
                ).ToList();
            }

            var pagination = new PaginationVM
            {
                CurrentPage = page,
                TotalItems = courseList.Count(),
                PageSize = 5,
                Query = query,
                StatusFilter = status?.ToString()
            };

            var paginatedCourses = courseList
                .Skip((page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            return View(new CoursesVM
            {
                Courses = paginatedCourses,
                Pagination = pagination
            });
        }

        #endregion





        #region Add Cource

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    var instructors = _instructor.Get(includes: [i => i.ApplicationUser]).ToList();

        //    var learningPaths = _learningPath.Get().ToList();

        //    ViewBag.Instructors = instructors;

        //    ViewBag.LearningPaths = learningPaths;

        //    return View(new Models.Course());
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Course course, IFormFile imageFile, int learningPathId)
        //{
        //    var instructor = _instructor.GetOne(i => i.Id == course.InstructorId);
        //    if (instructor == null)
        //    {
        //        ModelState.AddModelError("InstructorId", "Selected instructor does not exist.");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            // Handle image upload
        //            if (imageFile != null && imageFile.Length > 0)
        //            {
        //                // Create unique filename
        //                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
        //                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Assets", "Admin", "courses");

        //                // Create directory if it doesn't exist
        //                if (!Directory.Exists(uploadsFolder))
        //                {
        //                    Directory.CreateDirectory(uploadsFolder);
        //                }

        //                var filePath = Path.Combine(uploadsFolder, fileName);

        //                // Save file to server
        //                using (var stream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await imageFile.CopyToAsync(stream);
        //                }

        //                // Set image URL in the course object
        //                course.ImgUrl = $"/Assets/Admin/courses/{fileName}";
        //            }

        //            // Add course to database
        //            _course.Create(course);
        //            _course.SaveDB();

        //            // Associate with learning path if selected
        //            if (learningPathId > 0)
        //            {
        //                var courseLearningPath = new CourseLearningPath
        //                {
        //                    CourseId = course.Id,
        //                    LearningPathId = learningPathId
        //                };
        //                _courseLearningPath.Create(courseLearningPath);
        //                _courseLearningPath.SaveDB(); // Fixed: Changed from _learningPath to _courseLearningPath
        //            }

        //            TempData["SuccessMessage"] = "Course created successfully!";
        //            return RedirectToAction(nameof(Index));
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError("", "An error occurred while saving the course: " + ex.Message);
        //            _logger.LogError(ex, "Error creating course");
        //        }
        //    }

        //    // If we got this far, something failed; redisplay form with existing data
        //    ViewBag.Instructors = _instructor.Get()
        //        .Include(i => i.ApplicationUser)
        //        .ToList();
        //    ViewBag.LearningPaths = _learningPath.Get().ToList();

        //    return View(course);
        //}

        #endregion


    }
}
