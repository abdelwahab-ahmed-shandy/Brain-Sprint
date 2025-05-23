using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrainSprint.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class LearningPathsController : Controller
    {
        private readonly ILearningPathRepository _learningPathRepository;
        public LearningPathsController(ILearningPathRepository learningPathRepository)
        {
            _learningPathRepository = learningPathRepository;
        }

        public async Task<IActionResult> Index()
        {
            var paths = await _learningPathRepository.GetAsync(p => p.IsPublished == true);

            return View(paths);
        }

        public async Task<IActionResult> Details(string id)
        {
            var path = await _learningPathRepository.GetOneAsync(
                filter: p => p.Name.ToLower().Replace(" ", "-") == id,
                includes: new List<Expression<Func<LearningPath, object>>> { p => p.CourseLearningPaths },
                thenIncludes: new List<Func<IQueryable<LearningPath>, IQueryable<LearningPath>>>
                {
        q => q.Include(p => p.CourseLearningPaths).ThenInclude(clp => clp.Course)
                },
                tracked: false
            );

            if (path == null) return NotFound();

            return View(path);
        }


    }
}
