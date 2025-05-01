using System.Net.Sockets;
using System.Reflection;
using System.Xml.Linq;
//todo : THE BAG Error .
namespace DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Admin
            modelBuilder.Entity<Admin>()
                .HasOne(a => a.User)
                .WithOne(u => u.Admin)
                .HasForeignKey<Admin>(a => a.ApplicationUserId);

            // Student
            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(s => s.ApplicationUserId);

            // Instructor
            modelBuilder.Entity<Instructor>()
                .HasOne(i => i.ApplicationUser)
                .WithOne(u => u.Instructor)
                .HasForeignKey<Instructor>(i => i.ApplicationUserId);




        }

        #region Entities definition :



        #region Users and Permissions


        /*
         
        ## Users and Permissions ##

        ApplicationUsers => Represents registered users in the system

        Admins => System administrators or platform administrators

        Students => Registered students

        UserSessions => User sessions for tracking activity or logging in

        */


        // يمثل المستخدمين المسجلين في النظام
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        // مسؤولو النظام أو مدراء المنصة
        public DbSet<Admin> Admins { get; set; }

        // الطلاب المسجلين
        public DbSet<Student> Students { get; set; }

        // المعلمون أو المدربون
        public DbSet<Instructor> Instructors { get; set; }

        // جلسات المستخدمين لتتبع النشاط أو تسجيل الدخول
        public DbSet<UserSession> UserSessions { get; set; }

        #endregion



        #region Purchase And Order


        /*
        
        Carts => Represents the shopping carts of users

        CartItems => Represents the items inside the shopping carts

        Orders => Represents the orders made by users

        OrderItems => Represents the details of items within an order

         */


        // عربة التسوق الخاصة بالمستخدمين
        public DbSet<Cart> Carts { get; set; }

        // العناصر داخل عربة التسوق
        public DbSet<CartItem> CartItems { get; set; }

        // الطلبات التي قام بها المستخدمون
        public DbSet<Order> Orders { get; set; }

        // تفاصيل العناصر داخل الطلب
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


        // الكورسات التعليمية
        public DbSet<Course> Courses { get; set; }

        // الوحدات المكونة للكورسات
        public DbSet<CourseModule> CourseModules { get; set; }

        // اشتراك المستخدم في كورس معين
        public DbSet<EnrollmentCourse> EnrollmentCourses { get; set; }

        // الشهادات الممنوحة بعد إتمام الكورسات
        public DbSet<Certificate> Certificates { get; set; }

        // مراجعات وتقييمات الكورسات
        public DbSet<CourseReview> CourseReviews { get; set; }

        #endregion



        #region Learning Pathways


        /*
        
        LearningPaths => Represents learning paths that consist of multiple courses

        CourseLearningPaths => Represents the relationship between courses and learning paths

         */


        // مسارات تعلم تتضمن عدة كورسات
        public DbSet<LearningPath> LearningPaths { get; set; }

        // العلاقة بين الكورسات ومسارات التعلم
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


        // العقد التعليمية (محتوى مرن)
        public DbSet<Node> Nodes { get; set; }

        // العقد التي تحتوي على نصوص
        public DbSet<TextNode> TextNodes { get; set; }

        // العقد التي تحتوي على فيديوهات
        public DbSet<VideoNode> VideoNodes { get; set; }

        // المرفقات الخاصة بكل عقدة
        public DbSet<NodeAttachment> NodeAttachments { get; set; }

        // تتبع العقد التي شاهدها المستخدم
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



        // الامتحانات داخل المنصة
        public DbSet<Exam> Exams { get; set; }

        // الأسئلة المستخدمة في الامتحانات
        public DbSet<Question> Questions { get; set; }

        // الخيارات المرتبطة بكل سؤال
        public DbSet<Choice> Choices { get; set; }

        // محاولات أداء الامتحانات من قبل المستخدمين
        public DbSet<UserExamAttemp> UserExamAttemps { get; set; }

        // إجابات المستخدمين على الأسئلة
        public DbSet<UserAnswer> UserAnswers { get; set; }


        #endregion



        #region Badges and Achievements


        /*
         
        Badges => Represents the badges that users can earn

        UsersBadges => Represents the relationship between users and the badges they have earned
         
         */


        // الشارات التي يمكن للمستخدمين الحصول عليها
        public DbSet<Badge> Badges { get; set; }

        // العلاقة بين المستخدمين والشارات التي حصلوا عليها
        public DbSet<UsersBadge> UsersBadges { get; set; }

        #endregion



        #region Technical Support


        /*
         
        Tickets => Represents the tickets submitted by users to the support team

        TicketResponses => Represents the responses from the support team to the tickets

         */


        // التذاكر التي يرسلها المستخدمون لفريق الدعم
        public DbSet<Ticket> Tickets { get; set; }

        // ردود الدعم على التذاكر
        public DbSet<TicketResponse> TicketResponses { get; set; }


        #endregion



        #endregion


        #region Seed Data In Table :






        #endregion




    }
}
