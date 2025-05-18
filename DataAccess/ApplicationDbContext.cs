using Models.Enums;
using System.Net.Sockets;
using System.Reflection;
using System.Xml.Linq;

namespace DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {


        #region  Entities definition :



        #region Activity Logs
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        #endregion



        #region Users and Permissions


        /*
         
        ## Users and Permissions ##

        ApplicationUsers => Represents registered users in the system

        Admins => System administrators or platform administrators

        Students => Registered students

        UserSessions => User sessions for tracking activity or logging in

        */


        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Instructor> Instructors { get; set; }

        public DbSet<UserSession> UserSessions { get; set; }

        #endregion



        #region Purchase And Order


        /*
        
        Carts => Represents the shopping carts of users

        CartItems => Represents the items inside the shopping carts

        Orders => Represents the orders made by users

        OrderItems => Represents the details of items within an order

         */


        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }


        #endregion



        #region Education and courses


        /*
         
        Courses => Represents the courses available on the platform
        
        CourseModules => Represents the modules that make up a course

        EnrollmentCourses => Represents the enrollment of users in specific courses

        Certificates => Represents the certificates awarded upon course completion

        CourseReviews => Represents the reviews and ratings given to courses

         */


        public DbSet<Course> Courses { get; set; }

        public DbSet<CourseModule> CourseModules { get; set; }

        public DbSet<EnrollmentCourse> EnrollmentCourses { get; set; }

        public DbSet<Certificate> Certificates { get; set; }

        public DbSet<CourseReview> CourseReviews { get; set; }

        #endregion



        #region Learning Pathways


        /*
        
        LearningPaths => Represents learning paths that consist of multiple courses

        CourseLearningPaths => Represents the relationship between courses and learning paths

         */


        public DbSet<LearningPath> LearningPaths { get; set; }

        public DbSet<CourseLearningPath> CourseLearningPaths { get; set; }

        #endregion



        #region Educational content (nodes)


        /*
        
        Nodes => Represents the educational content (flexible content)

        TextNodes => Represents nodes that contain text

        VideoNodes => Represents nodes that contain videos

        NodeAttachments => Represents attachments related to each node

        UsersWatchedNodes => Represents the nodes watched by users

         */


        public DbSet<Node> Nodes { get; set; }

        public DbSet<TextNode> TextNodes { get; set; }

        public DbSet<VideoNode> VideoNodes { get; set; }

        public DbSet<NodeAttachment> NodeAttachments { get; set; }

        public DbSet<UsersWatchedNode> UsersWatchedNodes { get; set; }


        #endregion



        #region Tests and questions 


        /*
        
        Exams => Represents the tests available on the platform

        Questions => Questions used in exams

        Choices => Options associated with each question

        UserExamAttemps => Represents attempts made by users to take exams

        UserAnswers => Represents the answers provided by users for questions

         */



        public DbSet<Exam> Exams { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Choice> Choices { get; set; }

        public DbSet<UserExamAttemp> UserExamAttemps { get; set; }

        public DbSet<UserAnswer> UserAnswers { get; set; }


        #endregion



        #region Badges and Achievements


        /*
         
        Badges => Represents the badges that users can earn

        UsersBadges => Represents the relationship between users and the badges they have earned
         
         */


        public DbSet<Badge> Badges { get; set; }

        public DbSet<UsersBadge> UsersBadges { get; set; }

        #endregion



        #region Technical Support


        /*
         
        Tickets => Represents the tickets submitted by users to the support team

        TicketResponses => Represents the responses from the support team to the tickets

         */


        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<TicketResponse> TicketResponses { get; set; }


        #endregion



        #region Audit Logs
        public DbSet<AuditRecord> AuditRecords { get; set; }

        #endregion

        #endregion


        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }


        /// <summary>
        /// Create relationships and table settings when creating the model.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            #region Relations :

            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<Exam>(entity =>
            {
                entity.HasOne(e => e.CourseModule)
                        .WithMany(cm => cm.Exams)
                        .HasForeignKey(e => e.CourseModuleId)
                        .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasOne(q => q.Exam)
                    .WithMany(e => e.Questions)
                    .HasForeignKey(q => q.ExamId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Choice>(entity =>
            {
                entity.HasOne(c => c.Question)
                    .WithMany(q => q.Choices)
                    .HasForeignKey(c => c.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserAnswer>(entity =>
            {
                entity.HasOne(ua => ua.UserExamAttemp)
                    .WithMany(ue => ue.UserAnswers)
                    .HasForeignKey(ua => ua.UserExamAttempId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ua => ua.Question)
                    .WithMany(q => q.UserAnswers)
                    .HasForeignKey(ua => ua.QuestionId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ua => ua.Choice)
                    .WithMany(c => c.UserAnswers)
                    .HasForeignKey(ua => ua.ChoiceId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<CourseLearningPath>()
                    .HasOne(clp => clp.LearningPath)
                    .WithMany(lp => lp.CourseLearningPaths)
                    .HasForeignKey(clp => clp.LearningPathId)
                    .OnDelete(DeleteBehavior.Restrict);

            #endregion


        }
    }
}
