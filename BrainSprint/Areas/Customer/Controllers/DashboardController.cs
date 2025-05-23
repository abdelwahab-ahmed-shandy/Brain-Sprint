namespace BrainSprint.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class DashboardController : Controller
    {
        private IEnrollmentCourseService _courseService;

        public DashboardController(IEnrollmentCourseService courseService)
        {
            _courseService = courseService;
        }




    }
}
