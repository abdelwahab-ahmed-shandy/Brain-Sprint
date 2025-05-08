using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class AdminDashboardVM
    {
        // User statistics 
        public int TotalUsers { get; set; }
        public int TotalStudents { get; set; }
        public int TotalInstructors { get; set; }
        public int TotalAdmins { get; set; }
        public int TotalSuperAdmins { get; set; }
        public int NewUsersThisWeek { get; set; }
        public int ActiveUsersToday { get; set; }

        // Educational content statistics 
        public int TotalCourses { get; set; }
        public int ActiveCourses { get; set; }
        public int TotalLearningPaths { get; set; }
        public int TotalCourseLearningPaths { get; set; }
        public int NewContentThisWeek { get; set; }

        // Reviews and assessments
        public int TotalCourseReviews { get; set; }

        // Gamification
        public int TotalBadges { get; set; }
        public int TotalUserBadges { get; set; }
        public int BadgesAwardedThisWeek { get; set; }
        public string MostPopularBadge { get; set; }

        // Enrollment and certificates
        public int TotalEnrollments { get; set; }
        public int TotalCertificatesIssued { get; set; }

        // Orders and sales
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int CancelledOrders { get; set; }

        public int TotalOrderItems { get; set; }
        public int TotalCarts { get; set; }
        public int TotalCartItems { get; set; }
        public double TotalRevenue { get; set; }
        public int TotalCartItemsInCompletedCarts { get; set; }


        // Support system
        public int TotalTickets { get; set; }
        public int OpenTickets { get; set; }
        public int TotalTicketResponses { get; set; }
        public int HighPriorityTickets { get; set; }

        // Activity data
        public IEnumerable<ActivityLogDashboardVM> RecentActivities { get; set; }
        public IEnumerable<CourseDashboardVM> PopularCourses { get; set; } = new List<CourseDashboardVM>();
        public IEnumerable<LeaderboardDashboardUser> TopLearners { get; set; } = new List<LeaderboardDashboardUser>();
        public IEnumerable<BadgeDashboardVM> RecentBadges { get; set; } = new List<BadgeDashboardVM>();
    }

    public class LeaderboardDashboardUser
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
        public long TotalPonit { get; set; }
        public int Level { get; set; }
        public int BadgeCount { get; set; }
        public IEnumerable<BadgeDashboardVM> RecentBadges { get; set; }
    }

    public class ActivityLogDashboardVM
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserName { get; set; }
        public string UserAvatar { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public ActivityStatus Status { get; set; }
    }
    public class CourseDashboardVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string InstructorName { get; set; }
        public int EnrollmentCount { get; set; }
        public double? AverageRating { get; set; }
    }

    public class BadgeDashboardVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public DateTime AwardedDate { get; set; }
    }
}