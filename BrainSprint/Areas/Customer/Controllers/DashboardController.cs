
namespace BrainSprint.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Student")]
    public class DashboardController : Controller
    {
        private IEnrollmentCourseService _courseService;

        public DashboardController(IEnrollmentCourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IActionResult> Index()
        {
            var courses = _courseService.GetAll(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return View(await courses);
        }
    }
}
