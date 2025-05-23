using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace BrainSprint.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        #region Admin Dashboard 
        public async Task<IActionResult> Index()
        {
            var dashboard = new AdminDashboardVM
            {
                // User statistics
                TotalUsers = await _userManager.Users.CountAsync(),
                TotalStudents = (await _userManager.GetUsersInRoleAsync("Student")).Count,
                TotalInstructors = (await _userManager.GetUsersInRoleAsync("Instructor")).Count,
                TotalAdmins = (await _userManager.GetUsersInRoleAsync("Admin")).Count,
                TotalSuperAdmins = (await _userManager.GetUsersInRoleAsync("SuperAdmin")).Count,
                NewUsersThisWeek = await _context.Users
                    .CountAsync(u => u.CreatedDateUtc >= DateTime.UtcNow.AddDays(-7)),

                // Course statistics
                TotalCourses = await _context.Courses.CountAsync(),
                TotalLearningPaths = await _context.LearningPaths.CountAsync(),
                NewContentThisWeek = await _context.Courses
                    .CountAsync(c => c.CreatedDateUtc >= DateTime.UtcNow.AddDays(-7)),

                // Reviews
                TotalCourseReviews = await _context.CourseReviews.CountAsync(),

                // Gamification
                TotalBadges = await _context.Badges.CountAsync(),
                TotalUserBadges = await _context.UsersBadges.CountAsync(),
                BadgesAwardedThisWeek = await _context.UsersBadges
                    .CountAsync(b => b.AwardedDate >= DateTime.UtcNow.AddDays(-7)),
                MostPopularBadge = await _context.UsersBadges
                    .Include(ub => ub.Badge)
                    .GroupBy(ub => ub.BadgeId)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.First().Badge.Name)
                    .FirstOrDefaultAsync() ?? "No badges awarded yet",

                // Enrollment
                TotalEnrollments = await _context.EnrollmentCourses.CountAsync(),
                TotalCertificatesIssued = await _context.Certificates.CountAsync(),

                // Orders statistics
                TotalOrders = await _context.Orders.CountAsync(),
                PendingOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Pending),
                CancelledOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Canceled),
                CompletedOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Completed),

                // Revenue and carts
                TotalRevenue = await _context.Carts
                    .Where(c => c.CartStatus == CartStatusType.Completed)
                    .Include(c => c.CartItems)
                    .SelectMany(c => c.CartItems)
                    .SumAsync(ci => ci.PriceAtPurchase),
                TotalCartItemsInCompletedCarts = await _context.Carts
                    .Where(c => c.CartStatus == CartStatusType.Completed)
                    .SumAsync(c => c.CartItems.Count),

                // Support
                TotalTickets = await _context.Tickets.CountAsync(),
                OpenTickets = await _context.Tickets.CountAsync(t => t.Status == TicketStatusType.Opened),
                HighPriorityTickets = await _context.Tickets
                    .CountAsync(t => t.Priority == TicketPriorityType.High),

                // Activity data
                RecentActivities = await _context.ActivityLogs
                    .Include(a => a.User)
                    .OrderByDescending(a => a.Timestamp)
                    .Take(10)
                    .Select(a => new ActivityLogDashboardVM
                    {
                        Id = a.Id,
                        Timestamp = a.Timestamp,
                        UserName = a.User.UserName ?? "Unknown",
                        UserAvatar = a.User.ProfileImage ?? "/images/default-avatar.png",
                        Action = a.Action,
                        Details = a.Details,
                        Status = a.Status
                    })
                    .ToListAsync(),

                PopularCourses = await _context.Courses
                    .Where(c => c.IsPublished == true)
                    .Include(c => c.Instructor)
                        .ThenInclude(i => i.ApplicationUser)
                    .Include(c => c.EnrollmentCourses)
                    .Include(c => c.CourseReviews)
                    .OrderByDescending(c => c.EnrollmentCourses.Count)
                    .Take(5)
                    .Select(c => new CourseDashboardVM
                    {
                        Id = c.Id,
                        Title = c.Title,
                        ImageUrl = c.ImgUrl ?? "/images/default-course.png",
                        InstructorName = c.Instructor.ApplicationUser.UserName ?? "Unknown Instructor",
                        EnrollmentCount = c.EnrollmentCourses.Count,
                        AverageRating = c.CourseReviews.Any() ? c.CourseReviews.Average(r => r.Rating) : null
                    })
                    .ToListAsync(),

                TopLearners = await _context.Users
                        .Include(u => u.Student)
                            .ThenInclude(s => s.UsersBadges)
                                .ThenInclude(ub => ub.Badge)
                        .Where(u => u.TotalPoints > 0 && u.Student != null)
                        .OrderByDescending(u => u.TotalPoints)
                        .Take(5)
                        .Select(u => new LeaderboardDashboardUser
                        {
                            UserId = u.Id,
                            UserName = u.UserName,
                            AvatarUrl = u.ProfileImage ?? "/images/default-avatar.png",
                            TotalPonit = u.TotalPoints,
                            Level = u.Level,
                            BadgeCount = u.Student.UsersBadges.Count,
                            RecentBadges = u.Student.UsersBadges
                                .OrderByDescending(ub => ub.AwardedDate)
                                .Take(3)
                                .Select(ub => new BadgeDashboardVM
                                {
                                    Id = ub.Badge.Id,
                                    Name = ub.Badge.Name ?? "Unnamed Badge",
                                    ImageUrl = ub.Badge.ImageUrl ?? "/images/default-badge.png",
                                    AwardedDate = ub.AwardedDate
                                })
                                .ToList()
                        })
                        .AsNoTracking()
                        .ToListAsync()
            };

            return View(dashboard);
        }
        #endregion


    }
}
