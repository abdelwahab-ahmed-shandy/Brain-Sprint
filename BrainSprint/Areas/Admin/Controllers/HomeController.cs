using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrainSprint.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class HomeController : Controller
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IInstructorRepository _instructorRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseReviewRepository _courseReviewRepository;
        private readonly ICourseLearningPathRepository _courseLearningPathRepository;
        private readonly IBadgeRepository _badgeRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ICertificateRepository _certificateRepository;
        private readonly ITicketResponseRepository _ticketResponseRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IEnrollmentCourseRepository _enrollmentCourseRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IApplicationUserRepository applicationUserRepository, IInstructorRepository instructorRepository,
                                IAdminRepository adminRepository, IStudentRepository studentRepository, IActivityLogRepository activityLogRepository,
                                    ICourseRepository courseRepository, ICourseReviewRepository courseReviewRepository, ICourseLearningPathRepository courseLearningPathRepository,
                                        IBadgeRepository badgeRepository, ICartItemRepository cartItemRepository, ICertificateRepository certificateRepository,
                                            ITicketResponseRepository ticketResponseRepository, ITicketRepository ticketRepository, IEnrollmentCourseRepository enrollmentCourseRepository,
                                                IOrderItemRepository orderItemRepository, UserManager<ApplicationUser> userManager)
        {
            _applicationUserRepository = applicationUserRepository;
            _instructorRepository = instructorRepository;
            _adminRepository = adminRepository;
            _studentRepository = studentRepository;
            _activityLogRepository = activityLogRepository;
            _courseRepository = courseRepository;
            _courseReviewRepository = courseReviewRepository;
            _courseLearningPathRepository = courseLearningPathRepository;
            _badgeRepository = badgeRepository;
            _cartItemRepository = cartItemRepository;
            _certificateRepository = certificateRepository;
            _ticketResponseRepository = ticketResponseRepository;
            _ticketRepository = ticketRepository;
            _enrollmentCourseRepository = enrollmentCourseRepository;
            _orderItemRepository = orderItemRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            var totalUsers = await _userManager.Users.ToListAsync();

            var dashboard = new AdminDashboardViewModel
            {
                // User statistics
                TotalUsers = totalUsers.Count,
                //    TotalStudents = await _studentRepository.GetAsync().CountAsync(),
                //    TotalInstructors = await _instructorRepository.GetAsync().CountAsync(),
                //    TotalAdmins = await _adminRepository.GetAsync().CountAsync(),
                //    TotalSuperAdmins = totalUsers.Count(u => _userManager.GetRolesAsync(u).Result.Contains("SuperAdmin")),
                //    NewUsersThisWeek = await _applicationUserRepository.GetAsync(u =>
                //        u.CreatedDate >= DateTime.Now.AddDays(-7)).CountAsync(),
                //    ActiveUsersToday = await _applicationUserRepository.GetAsync(u =>
                //        u.LastLoginDate >= DateTime.Today).CountAsync(),

                //    // Educational content
                //    TotalCourses = await _courseRepository.GetAsync().CountAsync(),
                //    ActiveCourses = await _courseRepository.GetAsync(c => c.IsActive).CountAsync(),
                //    TotalLearningPaths = await _courseLearningPathRepository.GetAsync().CountAsync(),
                //    NewContentThisWeek = await _courseRepository.GetAsync(c =>
                //        c.CreatedDate >= DateTime.Now.AddDays(-7)).CountAsync(),

                //    // Reviews and assessments
                //    TotalCourseReviews = await _courseReviewRepository.GetAsync().CountAsync(),

                //    // Gamification
                //    TotalBadges = await _badgeRepository.GetAsync().CountAsync(),
                //    TotalUserBadges = await _badgeRepository.GetUserBadgesCountAsync(),
                //    BadgesAwardedThisWeek = await _badgeRepository.GetAsync(b =>
                //        b.AwardedDate >= DateTime.Now.AddDays(-7)).CountAsync(),
                //    MostPopularBadge = (await _badgeRepository.GetMostPopularBadgeAsync())?.Name,

                //    // Enrollment and certificates
                //    TotalEnrollments = await _enrollmentCourseRepository.GetAsync().CountAsync(),
                //    TotalCertificatesIssued = await _certificateRepository.GetAsync().CountAsync(),

                //    // Orders and sales
                //    TotalOrders = await _orderItemRepository.GetAsync().CountAsync(),
                //    PendingOrders = await _orderItemRepository.GetAsync(o =>
                //        o.Status == OrderStatus.Pending).CountAsync(),
                //    CompletedOrders = await _orderItemRepository.GetAsync(o =>
                //        o.Status == OrderStatus.Completed).CountAsync(),
                //    TotalCartItems = await _cartItemRepository.GetAsync().CountAsync(),

                //    // Support system
                //    TotalTickets = await _ticketRepository.GetAsync().CountAsync(),
                //    OpenTickets = await _ticketRepository.GetAsync(t =>
                //        t.Status == TicketStatus.Open).CountAsync(),
                //    HighPriorityTickets = await _ticketRepository.GetAsync(t =>
                //        t.Priority == TicketPriority.High).CountAsync(),

                //    // Activities
                //    RecentActivities = await _activityLogRepository.GetAsync(
                //orderBy: q => q.OrderByDescending(a => a.Timestamp),
                //take: 10),

                //    // Top learners
                //    TopLearners = await _badgeRepository.GetTopLearnersAsync(5),

                //    // Popular courses
                //    PopularCourses = await _courseRepository.GetPopularCoursesAsync(3)

            };

            return View(dashboard);
        }
    }
}
