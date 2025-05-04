using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class AdminDashboardViewModel
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
        public int TotalOrderItems { get; set; }
        public int TotalCarts { get; set; }
        public int TotalCartItems { get; set; }
        public decimal TotalRevenue { get; set; }

        // Support system
        public int TotalTickets { get; set; }
        public int OpenTickets { get; set; }
        public int TotalTicketResponses { get; set; }
        public int HighPriorityTickets { get; set; }

        // Activity data
        public IEnumerable<ActivityLog> RecentActivities { get; set; }
        public IEnumerable<LeaderboardUser> TopLearners { get; set; }
        public IEnumerable<Course> PopularCourses { get; set; }
    }

    public class LeaderboardUser
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
        public int TotalXP { get; set; }
        public int Level { get; set; }
        public int BadgeCount { get; set; }
        public IEnumerable<Badge> RecentBadges { get; set; }
    }
}