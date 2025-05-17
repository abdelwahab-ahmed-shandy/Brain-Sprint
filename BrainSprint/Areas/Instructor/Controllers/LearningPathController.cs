using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BrainSprint.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = "Instructor,Admin,SuperAdmin")]
    public class LearningPathController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IInstructorRepository _instructorRepository;
        private readonly ILearningPathRepository _learningPathRepository;
        public LearningPathController(UserManager<ApplicationUser> userManager, IInstructorRepository instructorRepository
            , ILearningPathRepository learningPathRepository)
        {
            _userManager = userManager;
            _instructorRepository = instructorRepository;
            _learningPathRepository = learningPathRepository;
        }


        #region View Learning Paths

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 5, string query = null, string statusFilter = null)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var currentInstructor = await _instructorRepository
                .Get(i => i.ApplicationUserId == user.Id)
                .FirstOrDefaultAsync();

            if (currentInstructor == null) return NotFound();

            // Await the repository call to get the actual collection
            var allLearningPaths = await _learningPathRepository
                .GetAsync(lp => lp.CourseLearningPaths.Any(clp => clp.Course.InstructorId == currentInstructor.Id),
                    includes: new List<Expression<Func<LearningPath, object>>>
                    {
                lp => lp.CourseLearningPaths
                    },
                    thenIncludes: new List<Func<IQueryable<LearningPath>, IQueryable<LearningPath>>>
                    {
                q => q.Include(lp => lp.CourseLearningPaths).ThenInclude(clp => clp.Course)
                    });

            // Apply search filter
            if (!string.IsNullOrEmpty(query))
            {
                allLearningPaths = allLearningPaths.Where(lp =>
                    lp.Name.Contains(query) ||
                    lp.Description.Contains(query));
            }

            // Apply status filter
            if (!string.IsNullOrEmpty(statusFilter))
            {
                allLearningPaths = statusFilter switch
                {
                    "Published" => allLearningPaths.Where(lp =>
                        lp.CourseLearningPaths.Any(clp => clp.Course.IsPublished == true)),
                    "Draft" => allLearningPaths.Where(lp =>
                        !lp.CourseLearningPaths.Any(clp => clp.Course.IsPublished == true)),
                    _ => allLearningPaths
                };
            }

            // Get total count before pagination
            var totalItems = allLearningPaths.Count();

            // Apply pagination
            var pagedLearningPaths = allLearningPaths
                .OrderBy(lp => lp.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Prepare view model
            var model = new LearningPathsVM
            {
                LearningPaths = pagedLearningPaths.Select(lp => new ContentManagementVM
                {
                    Id = lp.Id,
                    Name = lp.Name,
                    Description = lp.Description,
                    IconUrl = lp.IconUrl,
                    CreatedBy = lp.CreatedBy,
                    CreatedDateUtc = lp.CreatedDateUtc,
                    UpdatedBy = lp.UpdatedBy,
                    UpdatedDateUtc = lp.UpdatedDateUtc,
                    IsPublished = lp.CourseLearningPaths.Any(clp => clp.Course.IsPublished == true),
                    InstructorName = $"{currentInstructor.ApplicationUser.FirstName} {currentInstructor.ApplicationUser.LastName}"
                }),
                Pagination = new PaginationVM
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalItems,
                    Query = query,
                    StatusFilter = statusFilter
                }
            };

            return View(model);
        }

        #endregion


        // Start Here :::: 
        #region Create Learning Path


        [HttpGet]
        public IActionResult Create()
        {




            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContentManagementVM model)
        {
            if (ModelState.IsValid)
            {

            }
            return View();
        }

        #endregion



    }
}
