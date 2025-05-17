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


    }
}
