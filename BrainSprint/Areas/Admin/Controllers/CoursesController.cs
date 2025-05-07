using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;

namespace BrainSprint.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ("SuperAdmin,Admin"))]
    public class CoursesController : Controller
    {
        private readonly ICourseLearningPathRepository _courseLearningPath;
        private readonly ICourseRepository _course;
        private readonly ILearningPathRepository _Path;
        private readonly ILogger<CoursesController> _logger;
        private readonly IInstructorRepository _instructor;
        public CoursesController(ICourseLearningPathRepository courseLearningPathRepository, ICourseRepository courseRepository
                                    , IInstructorRepository instructorRepository, ILearningPathRepository learningPathRepository, ILogger<CoursesController> logger)
        {
            _course = courseRepository;
            _Path = learningPathRepository;
            _courseLearningPath = courseLearningPathRepository;
            _instructor = instructorRepository;
            _logger = logger;
        }


        #region View All Cources : 

        public async Task<IActionResult> Index(string? query, string? status, int page = 1)
        {
            var cources = _course.Get().ToList();

            var courseVMs = cources.Select(c => new ContentManagementVM
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                CreatedBy = c.CreatedBy,
                CreatedDateUtc = c.CreatedDateUtc,
                UpdatedBy = c.UpdatedBy,
                UpdatedDateUtc = c.UpdatedDateUtc,
                Price = c.Price,
                Discount = c.Discount,
                Duration = c.Duration,
                VideoUrl = c.VideoUrl,
                ImgUrl = c.ImgUrl
            });

            if (!string.IsNullOrEmpty(query))
            {
                courseVMs = courseVMs.Where(c =>
                    (c.Title?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (c.Description?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (c.CreatedBy?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (c.UpdatedBy?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (c.Duration.ToString().Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                    (c.Discount?.ToString().Contains(query) ?? false) ||
                    (c.CreatedDateUtc.ToString("yyyy-MM-dd").Contains(query)) ||
                    (c.UpdatedDateUtc?.ToString("yyyy-MM-dd").Contains(query) ?? false)
                ).ToList();
            }

            var pagination = new PaginationVM
            {
                CurrentPage = page,
                PageSize = 5,
                TotalItems = courseVMs.Count(),
                Query = query,
                StatusFilter = status
            };

            if (page > pagination.TotalPages && pagination.TotalPages > 0)
            {
                page = pagination.TotalPages;
                pagination.CurrentPage = page;
            }

            var paginatedCourses = courseVMs
                    .Skip((page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToList();


            return View(new CoursesVM
            {
                Pagination = pagination,
                Courses = paginatedCourses
            });

        }

        #endregion


        #region Add Cource


        #endregion








    }
}
