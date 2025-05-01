using Models.Enums;
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


        /// <summary>
        /// Create relationships and table settings when creating the model.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ApplicationUser
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.RegistrationDate)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(u => u.ProfileImage)
                    .HasMaxLength(500);

                entity.Property(u => u.TotalPoints)
                    .HasDefaultValue(0);

                entity.Property(u => u.Level)
                    .HasDefaultValue(1);

                entity.Property(u => u.IsActive)
                    .HasDefaultValue(true);

                entity.Property(u => u.IsBlocked)
                    .HasDefaultValue(false);

                entity.Property(u => u.AccountState)
                    .HasDefaultValue(AccountStateType.PendingActivation);
            });

            // Admin
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasOne(a => a.ApplicationUser)
                    .WithOne(u => u.Admin)
                    .HasForeignKey<Admin>(a => a.ApplicationUserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Badge
            modelBuilder.Entity<Badge>(entity =>
            {
                entity.Property(b => b.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(b => b.Description)
                    .HasMaxLength(500);

                entity.Property(b => b.ImageUrl)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            // Cart
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasOne(c => c.Student)
                    .WithMany(s => s.Carts)
                    .HasForeignKey(c => c.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(c => c.CartStatus)
                    .HasDefaultValue(CartStatusType.Active);
            });

            // CartItem
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasOne(ci => ci.Cart)
                    .WithMany(c => c.CartItems)
                    .HasForeignKey(ci => ci.CartId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ci => ci.Course)
                    .WithMany()
                    .HasForeignKey(ci => ci.CourseId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Certificate
            modelBuilder.Entity<Certificate>(entity =>
            {
                entity.HasOne(c => c.EnrollmentCourse)
                    .WithOne(ec => ec.Certificate)
                    .HasForeignKey<Certificate>(c => c.EnrollmantCourseId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(c => c.StudentScore)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            // Choice
            modelBuilder.Entity<Choice>(entity =>
            {
                entity.Property(c => c.OptionText)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.HasOne(c => c.Question)
                    .WithMany(q => q.Choices)
                    .HasForeignKey(c => c.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Course
            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(c => c.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(c => c.ShortDescription)
                    .HasMaxLength(500);

                entity.Property(c => c.Price)
                    .HasColumnType("decimal(18,2)");

                entity.Property(c => c.Discount)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(c => c.Instructor)
                    .WithMany(i => i.Courses)
                    .HasForeignKey(c => c.InstructorId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // CourseLearningPath
            modelBuilder.Entity<CourseLearningPath>(entity =>
            {
                entity.HasKey(clp => new { clp.CourseId, clp.LearningPathId });

                entity.HasOne(clp => clp.Course)
                    .WithMany(c => c.CourseLearningPaths)
                    .HasForeignKey(clp => clp.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(clp => clp.LearningPath)
                    .WithMany(lp => lp.CourseLearningPaths)
                    .HasForeignKey(clp => clp.LearningPathId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // CourseModule
            modelBuilder.Entity<CourseModule>(entity =>
            {
                entity.Property(cm => cm.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(cm => cm.Course)
                    .WithMany(c => c.CourseModules)
                    .HasForeignKey(cm => cm.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // CourseReview
            modelBuilder.Entity<CourseReview>(entity =>
            {
                entity.HasOne(cr => cr.Student)
                    .WithMany(s => s.CourseReviews)
                    .HasForeignKey(cr => cr.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(cr => cr.Course)
                    .WithMany(c => c.CourseReviews)
                    .HasForeignKey(cr => cr.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(cr => cr.Rating)
                    .IsRequired();
            });

            // EnrollmentCourse
            modelBuilder.Entity<EnrollmentCourse>(entity =>
            {
                entity.Property(ec => ec.EnrollmentDate)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(ec => ec.Student)
                    .WithMany(s => s.EnrollmentCourses)
                    .HasForeignKey(ec => ec.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ec => ec.Course)
                    .WithMany(c => c.EnrollmentCourses)
                    .HasForeignKey(ec => ec.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Exam
            modelBuilder.Entity<Exam>(entity =>
            {
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(e => e.CourseModule)
                    .WithOne(cm => cm.Exam)
                    .HasForeignKey<Exam>(e => e.CourseModuleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Instructor
            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.Property(i => i.IsVerified)
                    .HasDefaultValue(false);

                entity.HasOne(i => i.ApplicationUser)
                    .WithOne(u => u.Instructor)
                    .HasForeignKey<Instructor>(i => i.ApplicationUserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // LearningPath
            modelBuilder.Entity<LearningPath>(entity =>
            {
                entity.Property(lp => lp.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(lp => lp.Description)
                    .IsRequired()
                    .HasMaxLength(1000);
            });

            // Node
            modelBuilder.Entity<Node>(entity =>
            {
                entity.Property(n => n.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(n => n.CourseModule)
                    .WithMany(cm => cm.Nodes)
                    .HasForeignKey(n => n.CourseModuleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // NodeAttachment
            modelBuilder.Entity<NodeAttachment>(entity =>
            {
                entity.Property(na => na.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(na => na.FileUrl)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(na => na.Node)
                    .WithMany(n => n.NodeAttachments)
                    .HasForeignKey(na => na.NodeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(o => o.TotalAmount)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(o => o.Student)
                    .WithMany(s => s.Orders)
                    .HasForeignKey(o => o.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // OrderItem
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.Property(oi => oi.Price)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(oi => oi.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(oi => oi.Course)
                    .WithMany()
                    .HasForeignKey(oi => oi.CourseId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Question
            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(q => q.QuestionText)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.HasOne(q => q.Exam)
                    .WithMany(e => e.Questions)
                    .HasForeignKey(q => q.ExamId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Student
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasOne(s => s.ApplicationUser)
                    .WithOne(u => u.Student)
                    .HasForeignKey<Student>(s => s.ApplicationUserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // TextNode
            modelBuilder.Entity<TextNode>(entity =>
            {
                entity.Property(tn => tn.Text)
                    .IsRequired();

                entity.HasOne(tn => tn.Node)
                    .WithOne(n => n.TextNode)
                    .HasForeignKey<TextNode>(tn => tn.NodeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Ticket
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(t => t.Description)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(t => t.CreateAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(t => t.Status)
                    .HasDefaultValue(TicketStatusType.InProgress);

                entity.HasOne(t => t.User)
                    .WithMany(u => u.Tickets)
                    .HasForeignKey(t => t.ApplicationUserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // TicketResponse
            modelBuilder.Entity<TicketResponse>(entity =>
            {
                entity.Property(tr => tr.Message)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(tr => tr.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(tr => tr.Ticket)
                    .WithMany(t => t.TicketResponses)
                    .HasForeignKey(tr => tr.TicketId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(tr => tr.ResponderUser)
                    .WithMany()
                    .HasForeignKey(tr => tr.ResponderUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // UserAnswer
            modelBuilder.Entity<UserAnswer>(entity =>
            {
                entity.HasOne(ua => ua.UserExamAttemp)
                    .WithMany(uea => uea.UserAnswers)
                    .HasForeignKey(ua => ua.UserExamAttempId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ua => ua.Question)
                    .WithMany(q => q.UserAnswers)
                    .HasForeignKey(ua => ua.QuestionId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ua => ua.Choice)
                    .WithMany()
                    .HasForeignKey(ua => ua.ChoiceId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // UserExamAttemp
            modelBuilder.Entity<UserExamAttemp>(entity =>
            {
                entity.HasOne(uea => uea.Student)
                    .WithMany(s => s.UserExamAttemps)
                    .HasForeignKey(uea => uea.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(uea => uea.Exam)
                    .WithMany(e => e.UserExamAttemps)
                    .HasForeignKey(uea => uea.ExamId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // UsersBadge
            modelBuilder.Entity<UsersBadge>(entity =>
            {
                entity.HasKey(ub => new { ub.StudentId, ub.BadgeId });

                entity.Property(ub => ub.DateEarned)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(ub => ub.Student)
                    .WithMany(s => s.UsersBadges)
                    .HasForeignKey(ub => ub.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ub => ub.Badge)
                    .WithMany(b => b.UsersBadges)
                    .HasForeignKey(ub => ub.BadgeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // UserSession
            modelBuilder.Entity<UserSession>(entity =>
            {
                entity.HasKey(us => us.SessionId);

                entity.Property(us => us.LoginTime)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(us => us.IsActive)
                    .HasDefaultValue(true);

                entity.Property(us => us.IpAddress)
                    .HasMaxLength(50);

                entity.Property(us => us.DeviceInfo)
                    .HasMaxLength(500);
            });

            // UsersWatchedNode
            modelBuilder.Entity<UsersWatchedNode>(entity =>
            {
                entity.HasKey(uwn => new { uwn.StudentId, uwn.NodeId });

                entity.Property(uwn => uwn.IsWatched)
                    .HasDefaultValue(false);

                entity.HasOne(uwn => uwn.Student)
                    .WithMany(s => s.UsersWatchedNodes)
                    .HasForeignKey(uwn => uwn.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(uwn => uwn.Node)
                    .WithMany(n => n.UsersWatchedNodes)
                    .HasForeignKey(uwn => uwn.NodeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // VideoNode
            modelBuilder.Entity<VideoNode>(entity =>
            {
                entity.Property(vn => vn.VideoUrl)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(vn => vn.Node)
                    .WithOne(n => n.VideoNode)
                    .HasForeignKey<VideoNode>(vn => vn.NodeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
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
