using System.Net.Sockets;
using System.Reflection;
using System.Xml.Linq;

namespace DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }


        #region Entities definition :

        DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }

        //public DbSet<Badge> Badges { get; set; }
        //public DbSet<UsersBadge> UsersBadges { get; set; }

        //public DbSet<Cart> Carts { get; set; }
        //public DbSet<Order> Orders { get; set; }
        //public DbSet<CartItem> CartItems { get; set; }
        //public DbSet<OrderItem> OrderItems { get; set; }

        //public DbSet<Course> Courses { get; set; }
        //public DbSet<Certificate> Certificates { get; set; }
        //public DbSet<EnrollmentCourse> EnrollmentCourses { get; set; }


        //public DbSet<Question> Questions { get; set; }
        //public DbSet<Choice> Choices { get; set; }
        //public DbSet<Exam> Exams { get; set; }

        //public DbSet<CourseLearningPath> CourseLearningPaths { get; set; }
        //public DbSet<LearningPath> LearningPaths { get; set; }

        //public DbSet<Student> Students { get; set; }
        //public DbSet<Instructor> Instructors { get; set; }
        //public DbSet<CourseReview> CourseReviews { get; set; }

        //public DbSet<UserAnswer> UserAnswers { get; set; }
        //public DbSet<UserExamAttemp> UserExamAttemps { get; set; }
        //public DbSet<UsersWatchedNode> UsersWatchedNodes { get; set; }

        //public DbSet<Ticket> Tickets { get; set; }
        //public DbSet<TicketResponse> TicketResponses { get; set; }

        //public DbSet<Node> Nodes { get; set; }
        //public DbSet<Module> Modules { get; set; }
        //public DbSet<TextNode> TextNodes { get; set; }
        //public DbSet<VideoNode> VideoNodes { get; set; }
        //public DbSet<NodeAttachment> NodeAttachments { get; set; }



        #endregion




    }
}
