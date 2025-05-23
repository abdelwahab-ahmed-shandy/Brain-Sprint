using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class StudentDashboardVM
    {

        // Basic student info
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public LevelType? Level { get; set; }

        // Courses enrolled with progress and completion status
        public List<CourseProgressInfo> EnrolledCourses { get; set; } = new();

        // Badges earned
        public List<BadgeInfo> Badges { get; set; } = new();

        // Orders summary
        public List<OrderInfo> Orders { get; set; } = new();

        // Exam attempts with scores and dates
        public List<ExamAttemptInfo> ExamAttempts { get; set; } = new();

        // Watched nodes summary
        public int TotalNodesWatched { get; set; }
        public int TotalNodes { get; set; }

        // Cart info
        public int CartItemsCount { get; set; }
        public double CartTotalPrice { get; set; }

        // Reviews given
        public List<CourseReviewInfo> Reviews { get; set; } = new();

        // Nested classes for item details
        public class CourseProgressInfo
        {
            public int CourseId { get; set; }
            public string CourseTitle { get; set; } = string.Empty;
            public int Progress { get; set; }
            public bool IsCompleted { get; set; }
        }

        public class BadgeInfo
        {
            public int BadgeId { get; set; }
            public string BadgeName { get; set; } = string.Empty;
            public DateTime AwardedDate { get; set; }
        }

        public class OrderInfo
        {
            public int OrderId { get; set; }
            public DateTime OrderDate { get; set; }
            public double TotalAmount { get; set; }
            public OrderStatus Status { get; set; }
        }

        public class ExamAttemptInfo
        {
            public int ExamId { get; set; }
            public string ExamTitle { get; set; } = string.Empty;
            public int UserScore { get; set; }
            public DateTime StartedAt { get; set; }
            public DateTime FinishedAt { get; set; }
        }

        public class CourseReviewInfo
        {
            public int CourseId { get; set; }
            public string CourseTitle { get; set; } = string.Empty;
            public int Rating { get; set; }
            public string? Comment { get; set; }
        }
    }
}
