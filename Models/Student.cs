
namespace Models
{
    public class Student : BaseModel
    {
        public LevelType? Level { get; set; }


        [ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null!;


        public List<Cart> Carts { get; set; } = new List<Cart>();
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public List<CourseReview> CourseReviews { get; set; } = new List<CourseReview>();
        public List<EnrollmentCourse> EnrollmentCourses { get; set; } = new List<EnrollmentCourse>();
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<UserExamAttemp> UserExamAttemps { get; set; } = new List<UserExamAttemp>();
        public List<UsersBadge> UsersBadges { get; set; } = new List<UsersBadge>();
        public List<UsersWatchedNode> UsersWatchedNodes { get; set; } = new List<UsersWatchedNode>();
    }
}