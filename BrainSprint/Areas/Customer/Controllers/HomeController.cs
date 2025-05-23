using Models;
using Models.ViewModels;
using static Models.ViewModels.ContentManagementVM;

namespace BrainSprint.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ICourseReviewRepository courseReviewRepository;
        private readonly ILearningPathRepository _learningPathRepository;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ICourseReviewRepository courseReviewRepository)
        {
            this.courseReviewRepository = courseReviewRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

    }

}
