using Models;
using Models.ViewModels;
using static Models.ViewModels.ContentManagementVM;

namespace BrainSprint.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ICourseReviewRepository courseReviewRepository;

        public HomeController(ICourseReviewRepository courseReviewRepository)
        {
            this.courseReviewRepository = courseReviewRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        //[HttpGet]
        //public IActionResult Review()
        //{
        //    var reviews = courseReviewRepository.Get(includes: [e => e.Course]).ToList();
        //    ViewBag.Courses = courseReviewRepository.Get().ToList();

        //    var reviesVm = reviews.Select(e => new ContentManagementVM
        //    {
        //        Id = e.Id,
        //        CreatedBy = e.CreatedBy ?? "Anonymous",
        //        CreatedDateUtc = e.CreatedDateUtc,
        //        Rating = e.Rating,
        //        Comment = e.Comment ?? string.Empty,
        //        CourseId = e.CourseId,
        //        Course = e.Course ?? new Course { Title = "Unknown Course" }

        //    }).ToList();

        //    return View(reviesVm);
        //}
    }

}
