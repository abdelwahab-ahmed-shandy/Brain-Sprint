
using Microsoft.EntityFrameworkCore;

namespace BrainSprint.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = "Instructor,Admin,SuperAdmin")]

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentInstructor = await _context.Instructors
                .FirstOrDefaultAsync(i => i.ApplicationUserId == currentUser.Id);

            if (currentInstructor == null)
            {
                return NotFound();
            }

            var coursesTask = _context.Courses
                .Where(c => c.InstructorId == currentInstructor.Id)
                .AsNoTracking()
                .ToListAsync();


            var courses = await coursesTask;

            var instructorVM = new InstructorDashboardVM
            {
                TotalCourses = courses.Count,
                TotalLearningPaths = await _context.CourseLearningPaths
                    .Where(clp => clp.Course.InstructorId == currentInstructor.Id)
                    .CountAsync(),
                TotalStudents = await _context.EnrollmentCourses
                    .Where(ec => ec.Course.InstructorId == currentInstructor.Id)
                    .Select(ec => ec.StudentId)
                    .Distinct()
                    .CountAsync(),
                TotalReviews = await _context.CourseReviews
                    .Where(cr => cr.Course.InstructorId == currentInstructor.Id)
                    .CountAsync(),


                TotalCertificatesIssued = await _context.Certificates
                                            .Include(c => c.EnrollmentCourse)
                                                .ThenInclude(ec => ec.Course)
                                            .Where(c => c.EnrollmentCourse.Course.InstructorId == currentInstructor.Id
                                                   && c.EnrollmentCourse.Progress.HasValue
                                                   && c.EnrollmentCourse.Progress.Value == 100)
                                            .CountAsync(),


                TotalEnrollments = await _context.EnrollmentCourses
                    .Where(ec => ec.Course.InstructorId == currentInstructor.Id)
                    .CountAsync(),


                TotalOrders = await _context.Orders
                    .Where(o => o.OrderItems.Any(oi => oi.Course.InstructorId == currentInstructor.Id))
                    .CountAsync(),


                PendingOrders = await _context.Orders
                    .Where(o => o.Status == OrderStatus.Pending &&
                           o.OrderItems.Any(oi => oi.Course.InstructorId == currentInstructor.Id))
                    .CountAsync(),


                CompletedOrders = await _context.Orders
                    .Where(o => o.Status == OrderStatus.Completed &&
                           o.OrderItems.Any(oi => oi.Course.InstructorId == currentInstructor.Id))
                    .CountAsync(),


                CanceledOrders = await _context.Orders
                    .Where(o => o.Status == OrderStatus.Canceled &&
                           o.OrderItems.Any(oi => oi.Course.InstructorId == currentInstructor.Id))
                    .CountAsync()
            };

            return View(instructorVM);
        }


        [HttpGet]
        public IActionResult Courses()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult analytics()
        {
            return View();
        }
    }
}
