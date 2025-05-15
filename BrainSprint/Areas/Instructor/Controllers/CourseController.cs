
namespace BrainSprint.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = "Instructor,Admin,SuperAdmin")]
    public class CourseController : Controller
    {
        private readonly ICourseRepository courseRepository;
        private readonly ILearningPathRepository learningPathRepository;
        private readonly ICourseLearningPathRepository courseLearningPathRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public CourseController(ICourseRepository courseRepository, ILearningPathRepository learningPathRepository, ICourseLearningPathRepository courseLearningPathRepository, UserManager<ApplicationUser> userManager)
        {
            this.courseRepository = courseRepository;
            this.learningPathRepository = learningPathRepository;
            this.courseLearningPathRepository = courseLearningPathRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var courses = courseRepository.Get().ToList();
            return View(courses);
        }

    }
}
