using Models.Enums;
using System.Net.Sockets;
using System.Reflection;
using System.Xml.Linq;

namespace DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {


        #region  Entities definition :



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

            #endregion

            /*
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

                entity.HasOne(t => t.ApplicationUser)
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
            */



            #region Seed Data In Table :

            //            var users = new List<ApplicationUser>
            //    {
            //        new ApplicationUser
            //        {
            //            UserName = "admin@example.com",
            //            Email = "admin@example.com",
            //            FirstName = "Ahmed",
            //            LastName = "Ali",
            //            RegistrationDate = DateTime.Now.AddYears(-1),
            //            LastLogin = DateTime.Now.AddDays(-3),
            //            ProfileImage = "admin.jpg",
            //            TotalPoints = 5000,
            //            Level = 10,
            //            Bio = "System Administrator with 5 years of experience in managing educational platforms",
            //            IsActive = true,
            //            AccountState = AccountStateType.Active,
            //            CreatedDateUtc = DateTime.UtcNow.AddYears(-1),
            //            EmailConfirmed = true
            //        },
            //        new ApplicationUser
            //        {
            //            UserName = "instructor1@example.com",
            //            Email = "instructor1@example.com",
            //            FirstName = "Mohammed",
            //            LastName = "Abdullah",
            //            RegistrationDate = DateTime.Now.AddMonths(-6),
            //            LastLogin = DateTime.Now.AddDays(-1),
            //            ProfileImage = "instructor1.jpg",
            //            TotalPoints = 3000,
            //            Level = 7,
            //            Bio = "Programming Instructor with Web Development Experience",
            //            IsActive = true,
            //            AccountState = AccountStateType.Active,
            //            CreatedDateUtc = DateTime.UtcNow.AddMonths(-6),
            //            EmailConfirmed = true
            //        },
            //        new ApplicationUser
            //        {
            //            UserName = "student1@example.com",
            //            Email = "student1@example.com",
            //            FirstName = "Sarah",
            //            LastName = "Khaled",
            //            RegistrationDate = DateTime.Now.AddMonths(-2),
            //            LastLogin = DateTime.Now,
            //            ProfileImage = "student1.jpg",
            //            TotalPoints = 800,
            //            Level = 3,
            //            Bio = "A computer science student interested in learning programming",
            //            IsActive = true,
            //            AccountState = AccountStateType.Active,
            //            CreatedDateUtc = DateTime.UtcNow.AddMonths(-2),
            //            EmailConfirmed = true
            //        },
            //        new ApplicationUser
            //        {
            //            UserName = "student2@example.com",
            //            Email = "student2@example.com",
            //            FirstName = "Lina",
            //            LastName = "Youssef",
            //            RegistrationDate = DateTime.Now.AddMonths(-1),
            //            LastLogin = DateTime.Now.AddDays(-2),
            //            ProfileImage = "student2.jpg",
            //            TotalPoints = 600,
            //            Level = 2,
            //            Bio = "Beginner developer passionate about mobile apps",
            //            IsActive = true,
            //            AccountState = AccountStateType.Active,
            //            CreatedDateUtc = DateTime.UtcNow.AddMonths(-1),
            //            EmailConfirmed = true
            //        },
            //        new ApplicationUser
            //        {
            //            UserName = "instructor2@example.com",
            //            Email = "instructor2@example.com",
            //            FirstName = "Hassan",
            //            LastName = "Saleh",
            //            RegistrationDate = DateTime.Now.AddMonths(-4),
            //            LastLogin = DateTime.Now.AddDays(-5),
            //            ProfileImage = "instructor2.jpg",
            //            TotalPoints = 2800,
            //            Level = 6,
            //            Bio = "Instructor specialized in database systems",
            //            IsActive = true,
            //            AccountState = AccountStateType.Active,
            //            CreatedDateUtc = DateTime.UtcNow.AddMonths(-4),
            //            EmailConfirmed = true
            //        },
            //        new ApplicationUser
            //        {
            //            UserName = "student3@example.com",
            //            Email = "student3@example.com",
            //            FirstName = "Omar",
            //            LastName = "Nasser",
            //            RegistrationDate = DateTime.Now.AddMonths(-3),
            //            LastLogin = DateTime.Now.AddDays(-7),
            //            ProfileImage = "student3.jpg",
            //            TotalPoints = 400,
            //            Level = 1,
            //            Bio = "Learning programming fundamentals",
            //            IsActive = true,
            //            AccountState = AccountStateType.Active,
            //            CreatedDateUtc = DateTime.UtcNow.AddMonths(-3),
            //            EmailConfirmed = true
            //        },
            //        new ApplicationUser
            //        {
            //            UserName = "student4@example.com",
            //            Email = "student4@example.com",
            //            FirstName = "Mona",
            //            LastName = "Ibrahim",
            //            RegistrationDate = DateTime.Now.AddDays(-20),
            //            LastLogin = DateTime.Now.AddDays(-1),
            //            ProfileImage = "student4.jpg",
            //            TotalPoints = 150,
            //            Level = 1,
            //            Bio = "High school student exploring computer science",
            //            IsActive = true,
            //            AccountState = AccountStateType.Active,
            //            CreatedDateUtc = DateTime.UtcNow.AddDays(-20),
            //            EmailConfirmed = true
            //        },
            //        new ApplicationUser
            //        {
            //            UserName = "admin2@example.com",
            //            Email = "admin2@example.com",
            //            FirstName = "Rania",
            //            LastName = "Zaki",
            //            RegistrationDate = DateTime.Now.AddYears(-2),
            //            LastLogin = DateTime.Now.AddDays(-10),
            //            ProfileImage = "admin2.jpg",
            //            TotalPoints = 4700,
            //            Level = 9,
            //            Bio = "Platform coordinator and system analyst",
            //            IsActive = true,
            //            AccountState = AccountStateType.Active,
            //            CreatedDateUtc = DateTime.UtcNow.AddYears(-2),
            //            EmailConfirmed = true
            //        },
            //        new ApplicationUser
            //        {
            //            UserName = "student5@example.com",
            //            Email = "student5@example.com",
            //            FirstName = "Khaled",
            //            LastName = "Othman",
            //            RegistrationDate = DateTime.Now.AddMonths(-1),
            //            LastLogin = DateTime.Now.AddDays(-4),
            //            ProfileImage = "student5.jpg",
            //            TotalPoints = 950,
            //            Level = 3,
            //            Bio = "Passionate about backend development",
            //            IsActive = true,
            //            AccountState = AccountStateType.Active,
            //            CreatedDateUtc = DateTime.UtcNow.AddMonths(-1),
            //            EmailConfirmed = true
            //        },
            //        new ApplicationUser
            //        {
            //            UserName = "instructor3@example.com",
            //            Email = "instructor3@example.com",
            //            FirstName = "Nour",
            //            LastName = "Fahmy",
            //            RegistrationDate = DateTime.Now.AddMonths(-5),
            //            LastLogin = DateTime.Now.AddDays(-3),
            //            ProfileImage = "instructor3.jpg",
            //            TotalPoints = 3200,
            //            Level = 8,
            //            Bio = "Instructor focusing on front-end technologies and UX design",
            //            IsActive = true,
            //            AccountState = AccountStateType.Active,
            //            CreatedDateUtc = DateTime.UtcNow.AddMonths(-5),
            //            EmailConfirmed = true
            //        }
            //    };

            //            var admins = new List<Admin>
            //{
            //    new Admin
            //    {
            //        ApplicationUserId = users[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddYears(-1)
            //    },
            //    new Admin
            //    {
            //        ApplicationUserId = users[3].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-10)
            //    },
            //    new Admin
            //    {
            //        ApplicationUserId = users[5].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-4)
            //    }
            //};

            //            var instructors = new List<Instructor>
            //{
            //new Instructor
            //{
            //Certifications = "Microsoft Certified Trainer, AWS Certified",
            //ExperienceYears = "8+ years",
            //Rating = 4,
            //IsVerified = true,
            //ApplicationUserId = users[1].Id,
            //CreatedDateUtc = DateTime.UtcNow.AddMonths(-6)
            //},
            //new Instructor
            //{
            //ExperienceYears = "6 years",
            //Rating = 5,
            //IsVerified = true,
            //ApplicationUserId = users[3].Id,
            //CreatedDateUtc = DateTime.UtcNow.AddMonths(-8)
            //},
            //new Instructor
            //{
            //Certifications = "Unity Certified Developer",
            //ExperienceYears = "5 Years",
            //Rating = 4,
            //IsVerified = false,
            //ApplicationUserId = users[4].Id,
            //CreatedDateUtc = DateTime.UtcNow.AddMonths(-5)
            //},
            //new Instructor
            //{
            //Certifications = "Oracle Certified, Microsoft SQL Expert",
            //ExperienceYears = "10 Years",
            //Rating = 5,
            //IsVerified = true,
            //ApplicationUserId = users[5].Id,
            //CreatedDateUtc = DateTime.UtcNow.AddMonths(-12)
            //},
            //new Instructor
            //{
            //Certifications = "Flutter Certified, Kotlin Pro",
            //ExperienceYears = "4 years",
            //Rating = 4,
            //IsVerified = false,
            //ApplicationUserId = users[6].Id,
            //CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //},
            //new Instructor
            //{
            //Certifications = "Algorithmic Thinker Certification",
            //ExperienceYears = "7 years",
            //Rating = 5,
            //IsVerified = true,
            //ApplicationUserId = users[7].Id,
            //CreatedDateUtc = DateTime.UtcNow.AddMonths(-9)
            //},
            //new Instructor
            //{
            //Certifications = "AWS Solutions Architect, Docker Pro",
            //ExperienceYears = "6 years",
            //Rating = 4,
            //IsVerified = true,
            //ApplicationUserId = users[8].Id,
            //CreatedDateUtc = DateTime.UtcNow.AddMonths(-4)
            //},
            //new Instructor
            //{
            //Certifications = "Adobe XD Expert, UX Mastery",
            //ExperienceYears = "5 Years",
            //Rating = 4,
            //IsVerified = false,
            //ApplicationUserId = users[9].Id,
            //CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //},
            //new Instructor
            //{
            //Certifications = "CS Teaching Certificate",
            //ExperienceYears = "9 Years",
            //Rating = 5,
            //IsVerified = true,
            //ApplicationUserId = users[2].Id,
            //CreatedDateUtc = DateTime.UtcNow.AddMonths(-7)
            //},
            //new Instructor
            //{
            //Certifications = "Cisco CCNA, CEH",
            //ExperienceYears = "8 Years",
            //Rating = 4,
            //IsVerified = true,
            //ApplicationUserId = users[0].Id, // Even a manager can be an instructor
            //CreatedDateUtc = DateTime.UtcNow.AddMonths(-10)
            //}
            //};

            //            var students = new List<Student>
            //{
            //    new Student
            //    {
            //        Level = LevelType.Beginner,
            //        ApplicationUserId = users[2].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new Student
            //    {
            //        Level = LevelType.Beginner,
            //        ApplicationUserId = users[3].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-1)
            //    },
            //    new Student
            //    {
            //        Level = LevelType.Gold,
            //        ApplicationUserId = users[4].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    },
            //    new Student
            //    {
            //        Level = LevelType.Bronz,
            //        ApplicationUserId = users[5].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-4)
            //    },
            //    new Student
            //    {
            //        Level = LevelType.Beginner,
            //        ApplicationUserId = users[6].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-1)
            //    },
            //    new Student
            //    {
            //        Level = LevelType.Gold,
            //        ApplicationUserId = users[7].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-6)
            //    },
            //    new Student
            //    {
            //        Level = LevelType.Beginner,
            //        ApplicationUserId = users[8].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddDays(-20)
            //    },
            //    new Student
            //    {
            //        Level = LevelType.Gold,
            //        ApplicationUserId = users[9].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-5)
            //    },
            //    new Student
            //    {
            //        Level = LevelType.Beginner,
            //        ApplicationUserId = users[1].Id, // المدرّس ممكن يكون طالب أيضًا
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-8)
            //    },
            //    new Student
            //    {
            //        Level = LevelType.Bronz,
            //        ApplicationUserId = users[0].Id, // المدير كذلك
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-10)
            //    }
            //};

            //            var courses = new List<Course>
            //{
            //    new Course
            //    {
            //        Title = "Learn C# Programming",
            //        ShortDescription = "A comprehensive course to learn the basics of programming using C#",
            //        LongDescription = "This course covers all the basics of the C# language from beginner to advanced level",
            //        Price = 299.99,
            //        Duration = TimeSpan.FromHours(15),
            //        ThumbnailUrl = "csharp-course.jpg",
            //        InstructorId = instructors[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-5)
            //    },
            //    new Course
            //    {
            //        Title = "Web Development Basics",
            //        ShortDescription = "Learn to build modern websites",
            //        LongDescription = "This course covers HTML, CSS, JavaScript, and modern web technologies",
            //        Price = 199.99,
            //        Duration = TimeSpan.FromHours(10),
            //        ThumbnailUrl = "web-dev.jpg",
            //        InstructorId = instructors[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    },
            //    new Course
            //    {
            //        Title = "Introduction to Data Science",
            //        ShortDescription = "Start your data science journey",
            //        LongDescription = "Learn data analysis, visualization, and introductory machine learning techniques",
            //        Price = 399.99,
            //        Duration = TimeSpan.FromHours(20),
            //        ThumbnailUrl = "data-science.jpg",
            //        InstructorId = instructors[1].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-4)
            //    },
            //    new Course
            //    {
            //        Title = "Game Development with Unity",
            //        ShortDescription = "Build your first 2D and 3D games",
            //        LongDescription = "Learn Unity fundamentals, physics, animation, and scripting",
            //        Price = 249.99,
            //        Duration = TimeSpan.FromHours(18),
            //        ThumbnailUrl = "unity.jpg",
            //        InstructorId = instructors[2].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-6)
            //    },
            //    new Course
            //    {
            //        Title = "Mastering SQL and Databases",
            //        ShortDescription = "Everything you need to work with data",
            //        LongDescription = "Covers SQL queries, joins, indexes, views, stored procedures, and database design",
            //        Price = 179.99,
            //        Duration = TimeSpan.FromHours(12),
            //        ThumbnailUrl = "sql-course.jpg",
            //        InstructorId = instructors[3].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new Course
            //    {
            //        Title = "Mobile App Development with Flutter",
            //        ShortDescription = "Create cross-platform mobile apps",
            //        LongDescription = "Build iOS and Android apps using Flutter and Dart from scratch",
            //        Price = 219.99,
            //        Duration = TimeSpan.FromHours(16),
            //        ThumbnailUrl = "flutter-course.jpg",
            //        InstructorId = instructors[4].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-1)
            //    },
            //    new Course
            //    {
            //        Title = "Data Structures and Algorithms",
            //        ShortDescription = "Foundational course for coding interviews",
            //        LongDescription = "Covers arrays, linked lists, trees, graphs, recursion, and complexity analysis",
            //        Price = 299.99,
            //        Duration = TimeSpan.FromHours(14),
            //        ThumbnailUrl = "algorithms.jpg",
            //        InstructorId = instructors[5].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-7)
            //    },
            //    new Course
            //    {
            //        Title = "DevOps Fundamentals",
            //        ShortDescription = "Automate and deploy your applications",
            //        LongDescription = "Learn CI/CD, Docker, Kubernetes, monitoring, and cloud infrastructure",
            //        Price = 349.99,
            //        Duration = TimeSpan.FromHours(22),
            //        ThumbnailUrl = "devops.jpg",
            //        InstructorId = instructors[6].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-8)
            //    },
            //    new Course
            //    {
            //        Title = "UI/UX Design Principles",
            //        ShortDescription = "Design stunning and usable interfaces",
            //        LongDescription = "Learn wireframing, prototyping, and UI tools like Figma and Adobe XD",
            //        Price = 159.99,
            //        Duration = TimeSpan.FromHours(9),
            //        ThumbnailUrl = "ui-ux.jpg",
            //        InstructorId = instructors[7].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddDays(-30)
            //    },
            //    new Course
            //    {
            //        Title = "Cybersecurity Essentials",
            //        ShortDescription = "Protect systems and networks",
            //        LongDescription = "Understand encryption, firewalls, ethical hacking, and security protocols",
            //        Price = 289.99,
            //        Duration = TimeSpan.FromHours(13),
            //        ThumbnailUrl = "cybersecurity.jpg",
            //        InstructorId = instructors[9].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    }
            //};

            //            var enrollments = new List<EnrollmentCourse>
            //{
            //    new EnrollmentCourse
            //    {
            //        EnrollmentDate = DateTime.Now.AddMonths(-1),
            //        StudentId = students[0].Id,
            //        CourseId = courses[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-1)
            //    },
            //    new EnrollmentCourse
            //    {
            //        EnrollmentDate = DateTime.Now.AddMonths(-2),
            //        StudentId = students[1].Id,
            //        CourseId = courses[1].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new EnrollmentCourse
            //    {
            //        EnrollmentDate = DateTime.Now.AddMonths(-3),
            //        StudentId = students[2].Id,
            //        CourseId = courses[2].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    },
            //    new EnrollmentCourse
            //    {
            //        EnrollmentDate = DateTime.Now.AddMonths(-1),
            //        StudentId = students[3].Id,
            //        CourseId = courses[3].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-1)
            //    },
            //    new EnrollmentCourse
            //    {
            //        EnrollmentDate = DateTime.Now.AddMonths(-4),
            //        StudentId = students[4].Id,
            //        CourseId = courses[4].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-4)
            //    },
            //    new EnrollmentCourse
            //    {
            //        EnrollmentDate = DateTime.Now.AddMonths(-5),
            //        StudentId = students[5].Id,
            //        CourseId = courses[5].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-5)
            //    },
            //    new EnrollmentCourse
            //    {
            //        EnrollmentDate = DateTime.Now.AddMonths(-6),
            //        StudentId = students[6].Id,
            //        CourseId = courses[6].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-6)
            //    },
            //    new EnrollmentCourse
            //    {
            //        EnrollmentDate = DateTime.Now.AddMonths(-2),
            //        StudentId = students[7].Id,
            //        CourseId = courses[7].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new EnrollmentCourse
            //    {
            //        EnrollmentDate = DateTime.Now.AddMonths(-3),
            //        StudentId = students[8].Id,
            //        CourseId = courses[8].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    },
            //    new EnrollmentCourse
            //    {
            //        EnrollmentDate = DateTime.Now.AddMonths(-4),
            //        StudentId = students[9].Id,
            //        CourseId = courses[9].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-4)
            //    }
            //};

            //            var courseModules = new List<CourseModule>
            //{
            //    new CourseModule
            //    {
            //        Title = "Introduction to C#",
            //        Index = 1,
            //        CourseId = courses[0].Id, // C# course
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-5)
            //    },
            //    new CourseModule
            //    {
            //        Title = "Object-Oriented Programming",
            //        Index = 2,
            //        CourseId = courses[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-5)
            //    },
            //    new CourseModule
            //    {
            //        Title = "LINQ and Collections",
            //        Index = 3,
            //        CourseId = courses[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-4)
            //    },
            //    new CourseModule
            //    {
            //        Title = "Advanced C# Concepts",
            //        Index = 4,
            //        CourseId = courses[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    },
            //    new CourseModule
            //    {
            //        Title = "Introduction to HTML",
            //        Index = 1,
            //        CourseId = courses[1].Id, // Web Development Course
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    },
            //    new CourseModule
            //    {
            //        Title = "CSS Basics",
            //        Index = 2,
            //        CourseId = courses[1].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    },
            //    new CourseModule
            //    {
            //        Title = "JavaScript Essentials",
            //        Index = 3,
            //        CourseId = courses[1].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new CourseModule
            //    {
            //        Title = "Responsive Web Design with Bootstrap",
            //        Index = 4,
            //        CourseId = courses[1].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new CourseModule
            //    {
            //        Title = "Building Dynamic Web Pages with PHP",
            //        Index = 5,
            //        CourseId = courses[1].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-1)
            //    },
            //    new CourseModule
            //    {
            //        Title = "Introduction to Data Science",
            //        Index = 1,
            //        CourseId = courses[2].Id, // Data Science Course
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-4)
            //    },
            //    new CourseModule
            //    {
            //        Title = "Data Preprocessing and Visualization",
            //        Index = 2,
            //        CourseId = courses[2].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    },
            //    new CourseModule
            //    {
            //        Title = "Machine Learning Basics",
            //        Index = 3,
            //        CourseId = courses[2].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new CourseModule
            //    {
            //        Title = "Advanced Machine Learning Algorithms",
            //        Index = 4,
            //        CourseId = courses[2].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-1)
            //    }
            //};

            //            var nodes = new List<Node>
            //{
            //    new Node
            //    {
            //        Title = "What is C#?",
            //        NodeType = NodeType.Video,
            //        Index = 1,
            //        IsFree = true, // The first node is free
            //        CourseModuleId = courseModules[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-5)
            //    },
            //    new Node
            //    {
            //        Title = "Install Visual Studio",
            //        NodeType = NodeType.Text,
            //        Index = 2,
            //        IsFree = false,
            //        CourseModuleId = courseModules[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-5)
            //    },
            //    new Node
            //    {
            //        Title = "Create your first project",
            //        NodeType = NodeType.Video,
            //        Index = 3,
            //        IsFree = false,
            //        CourseModuleId = courseModules[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-5)
            //    },
            //    new Node
            //    {
            //        Title = "Introduction to HTML",
            //        NodeType = NodeType.Video,
            //        Index = 1,
            //        IsFree = true,
            //        CourseModuleId = courseModules[4].Id, // Web Development Course
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    },
            //    new Node
            //    {
            //        Title = "CSS Basics - A Deep Dive",
            //        NodeType = NodeType.Text,
            //        Index = 2,
            //        IsFree = false,
            //        CourseModuleId = courseModules[5].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    },
            //    new Node
            //    {
            //        Title = "Building Responsive Web Pages",
            //        NodeType = NodeType.Video,
            //        Index = 3,
            //        IsFree = false,
            //        CourseModuleId = courseModules[6].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new Node
            //    {
            //        Title = "JavaScript Fundamentals",
            //        NodeType = NodeType.Text,
            //        Index = 4,
            //        IsFree = true,
            //        CourseModuleId = courseModules[7].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new Node
            //    {
            //        Title = "Advanced JavaScript Techniques",
            //        NodeType = NodeType.Video,
            //        Index = 5,
            //        IsFree = false,
            //        CourseModuleId = courseModules[7].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-1)
            //    },
            //    new Node
            //    {
            //        Title = "Intro to Data Visualization",
            //        NodeType = NodeType.Text,
            //        Index = 1,
            //        IsFree = true,
            //        CourseModuleId = courseModules[9].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-4)
            //    },
            //    new Node
            //    {
            //        Title = "Understanding Data Preprocessing",
            //        NodeType = NodeType.Video,
            //        Index = 2,
            //        IsFree = false,
            //        CourseModuleId = courseModules[10].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    }
            //};

            //            var videoNodes = new List<VideoNode>
            //{
            //    new VideoNode
            //    {
            //        VideoUrl = "https://example.com/videos/csharp-intro.mp4",
            //        Duration = TimeSpan.FromMinutes(15),
            //        NodeId = nodes[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-5)
            //    },
            //    new VideoNode
            //    {
            //        VideoUrl = "https://example.com/videos/install-visual-studio.mp4",
            //        Duration = TimeSpan.FromMinutes(10),
            //        NodeId = nodes[2].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-5)
            //    },
            //    new VideoNode
            //    {
            //        VideoUrl = "https://example.com/videos/first-project.mp4",
            //        Duration = TimeSpan.FromMinutes(12),
            //        NodeId = nodes[3].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-5)
            //    },
            //    new VideoNode
            //    {
            //        VideoUrl = "https://example.com/videos/html-intro.mp4",
            //        Duration = TimeSpan.FromMinutes(18),
            //        NodeId = nodes[4].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    },
            //    new VideoNode
            //    {
            //        VideoUrl = "https://example.com/videos/css-basics.mp4",
            //        Duration = TimeSpan.FromMinutes(20),
            //        NodeId = nodes[5].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    },
            //    new VideoNode
            //    {
            //        VideoUrl = "https://example.com/videos/responsive-web-design.mp4",
            //        Duration = TimeSpan.FromMinutes(25),
            //        NodeId = nodes[6].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new VideoNode
            //    {
            //        VideoUrl = "https://example.com/videos/javascript-basics.mp4",
            //        Duration = TimeSpan.FromMinutes(20),
            //        NodeId = nodes[7].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new VideoNode
            //    {
            //        VideoUrl = "https://example.com/videos/advanced-javascript.mp4",
            //        Duration = TimeSpan.FromMinutes(22),
            //        NodeId = nodes[8].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-1)
            //    },
            //    new VideoNode
            //    {
            //        VideoUrl = "https://example.com/videos/data-visualization.mp4",
            //        Duration = TimeSpan.FromMinutes(16),
            //        NodeId = nodes[9].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-4)
            //    },
            //    new VideoNode
            //    {
            //        VideoUrl = "https://example.com/videos/data-preprocessing.mp4",
            //        Duration = TimeSpan.FromMinutes(18),
            //        NodeId = nodes[10].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    }
            //};

            //            var textNodes = new List<TextNode>
            //{
            //    new TextNode
            //    {
            //        Text = "خطوات تثبيت Visual Studio:\n1. حمل المثبت من الموقع الرسمي\n2. شغل الملف\n3. اتبع التعليمات",
            //        Length = 500,
            //        NodeId = nodes[1].Id, // العقدة الثانية (نصية)
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-5)
            //    },
            //    new TextNode
            //    {
            //        Text = "مقدمة إلى HTML: HTML هو اللغة الأساسية لتطوير صفحات الويب. يتم استخدامه لإنشاء الهيكل الأساسي للصفحات، ويعتمد على الوسوم لعرض المحتوى.",
            //        Length = 450,
            //        NodeId = nodes[4].Id, // العقدة الخامسة (نصية)
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    },
            //    new TextNode
            //    {
            //        Text = "أساسيات CSS: CSS هي لغة تنسيق تُستخدم لتحديد تصميم الصفحات. يمكنك تخصيص الألوان، الخطوط، والمسافات باستخدامها.",
            //        Length = 350,
            //        NodeId = nodes[5].Id, // العقدة السادسة (نصية)
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    },
            //    new TextNode
            //    {
            //        Text = "مقدمة إلى JavaScript: JavaScript هي لغة برمجة تُستخدم لجعل الصفحات تفاعلية. يمكنك استخدامها للتحقق من المدخلات، والتفاعل مع المستخدم.",
            //        Length = 400,
            //        NodeId = nodes[7].Id, // العقدة الثامنة (نصية)
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new TextNode
            //    {
            //        Text = "التصميم المتجاوب باستخدام Bootstrap: Bootstrap هو إطار عمل يستخدم لبناء مواقع متجاوبة تعمل على جميع الأجهزة.",
            //        Length = 450,
            //        NodeId = nodes[6].Id, // العقدة السابعة (نصية)
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new TextNode
            //    {
            //        Text = "مقدمة إلى علوم البيانات: تعلم كيفية معالجة البيانات وتنظيمها لتصبح قابلة للتحليل. هذه المهارات أساسية في مجال البيانات.",
            //        Length = 470,
            //        NodeId = nodes[9].Id, // العقدة العاشرة (نصية)
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-4)
            //    },
            //    new TextNode
            //    {
            //        Text = "تحليل البيانات باستخدام أدوات مثل Python و R: تعلم كيفية استخدام أدوات البرمجة لتحليل وتنظيف البيانات.",
            //        Length = 430,
            //        NodeId = nodes[10].Id, // العقدة الحادية عشر (نصية)
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    },
            //    new TextNode
            //    {
            //        Text = "مقدمة في التعلم الآلي: تعلم كيفية تطبيق خوارزميات التعلم الآلي على البيانات. التعلم الآلي هو الأساس للكثير من التطبيقات الذكية.",
            //        Length = 500,
            //        NodeId = nodes[11].Id, // العقدة الثانية عشر (نصية)
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new TextNode
            //    {
            //        Text = "تحليل البيانات باستخدام Python: تعلم كيفية استخدام Python لتحليل البيانات. تعد Python واحدة من أشهر لغات البرمجة لهذا الغرض.",
            //        Length = 460,
            //        NodeId = nodes[12].Id, // العقدة الثالثة عشر (نصية)
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-1)
            //    },
            //    new TextNode
            //    {
            //        Text = "مقدمة إلى معالجة الصور باستخدام OpenCV: تعلم كيفية استخدام مكتبة OpenCV لمعالجة الصور وتطبيق الفلاتر.",
            //        Length = 480,
            //        NodeId = nodes[13].Id, // العقدة الرابعة عشر (نصية)
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-1)
            //    }
            //};

            //            var exams = new List<Exam>
            //{
            //    new Exam
            //    {
            //        Title = "Unit 1 Final Exam - Introduction to C#",
            //        Score = 100,
            //        MaximumTime = TimeSpan.FromMinutes(30),
            //        CourseModuleId = courseModules[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-4)
            //    },
            //    new Exam
            //    {
            //        Title = "Unit 2 Final Exam - Object-Oriented Programming",
            //        Score = 100,
            //        MaximumTime = TimeSpan.FromMinutes(30),
            //        CourseModuleId = courseModules[1].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    },
            //    new Exam
            //    {
            //        Title = "Unit 3 Final Exam - HTML Basics",
            //        Score = 100,
            //        MaximumTime = TimeSpan.FromMinutes(25),
            //        CourseModuleId = courseModules[2].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    },
            //    new Exam
            //    {
            //        Title = "Unit 4 Final Exam - CSS Basics",
            //        Score = 100,
            //        MaximumTime = TimeSpan.FromMinutes(30),
            //        CourseModuleId = courseModules[3].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new Exam
            //    {
            //        Title = "Unit 5 Final Exam - JavaScript Essentials",
            //        Score = 100,
            //        MaximumTime = TimeSpan.FromMinutes(35),
            //        CourseModuleId = courseModules[4].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new Exam
            //    {
            //        Title = "Unit 6 Final Exam - Responsive Web Design",
            //        Score = 100,
            //        MaximumTime = TimeSpan.FromMinutes(40),
            //        CourseModuleId = courseModules[5].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new Exam
            //    {
            //        Title = "Unit 7 Final Exam - Advanced JavaScript",
            //        Score = 100,
            //        MaximumTime = TimeSpan.FromMinutes(40),
            //        CourseModuleId = courseModules[6].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-1)
            //    },
            //    new Exam
            //    {
            //        Title = "Unit 8 Final Exam - Data Visualization",
            //        Score = 100,
            //        MaximumTime = TimeSpan.FromMinutes(30),
            //        CourseModuleId = courseModules[7].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-1)
            //    },
            //    new Exam
            //    {
            //        Title = "Unit 9 Final Exam - Machine Learning Basics",
            //        Score = 100,
            //        MaximumTime = TimeSpan.FromMinutes(45),
            //        CourseModuleId = courseModules[8].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-1)
            //    },
            //    new Exam
            //    {
            //        Title = "Unit 10 Final Exam - Data Science Introduction",
            //        Score = 100,
            //        MaximumTime = TimeSpan.FromMinutes(45),
            //        CourseModuleId = courseModules[9].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-1)
            //    }
            //};

            //            var questions = new List<Question>
            //{
            //    new Question
            //    {
            //        QuestionText = "What is C#?",
            //        IsCorrect = false,
            //        Score = 20,
            //        ExamId = exams[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-4)
            //    },
            //    new Question
            //    {
            //        QuestionText = "What is the main platform for running C# applications?",
            //        IsCorrect = false,
            //        Score = 30,
            //        ExamId = exams[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-4)
            //    },
            //    new Question
            //    {
            //        QuestionText = "What is the key concept behind Object-Oriented Programming?",
            //        IsCorrect = false,
            //        Score = 25,
            //        ExamId = exams[1].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    },
            //    new Question
            //    {
            //        QuestionText = "Which tag is used to create a hyperlink in HTML?",
            //        IsCorrect = false,
            //        Score = 20,
            //        ExamId = exams[2].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    },
            //    new Question
            //    {
            //        QuestionText = "What does CSS stand for?",
            //        IsCorrect = false,
            //        Score = 20,
            //        ExamId = exams[3].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new Question
            //    {
            //        QuestionText = "Which JavaScript function is used to display an alert box?",
            //        IsCorrect = false,
            //        Score = 25,
            //        ExamId = exams[4].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new Question
            //    {
            //        QuestionText = "What is Bootstrap used for in web development?",
            //        IsCorrect = false,
            //        Score = 20,
            //        ExamId = exams[5].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new Question
            //    {
            //        QuestionText = "What does JSON stand for?",
            //        IsCorrect = false,
            //        Score = 25,
            //        ExamId = exams[6].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-1)
            //    },
            //    new Question
            //    {
            //        QuestionText = "What is the primary goal of data visualization?",
            //        IsCorrect = false,
            //        Score = 30,
            //        ExamId = exams[7].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-1)
            //    },
            //    new Question
            //    {
            //        QuestionText = "Which of the following is a machine learning algorithm?",
            //        IsCorrect = false,
            //        Score = 30,
            //        ExamId = exams[8].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-1)
            //    }
            //};

            //            var choices = new List<Choice>
            //{
            //    new Choice
            //    {
            //        OptionText = "Object-oriented programming language",
            //        IsCorrect = true,
            //        QuestionId = questions[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-4)
            //    },
            //    new Choice
            //    {
            //        OptionText = "Scripting language",
            //        IsCorrect = false,
            //        QuestionId = questions[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-4)
            //    },
            //    new Choice
            //    {
            //        OptionText = "CLR (Common Language Runtime)",
            //        IsCorrect = true,
            //        QuestionId = questions[1].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-4)
            //    },
            //    new Choice
            //    {
            //        OptionText = "JVM (Java Virtual Machine)",
            //        IsCorrect = false,
            //        QuestionId = questions[1].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-4)
            //    },
            //    new Choice
            //    {
            //        OptionText = "Encapsulation",
            //        IsCorrect = true,
            //        QuestionId = questions[2].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    },
            //    new Choice
            //    {
            //        OptionText = "Abstraction",
            //        IsCorrect = false,
            //        QuestionId = questions[2].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    },
            //    new Choice
            //    {
            //        OptionText = "<a>",
            //        IsCorrect = true,
            //        QuestionId = questions[3].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    },
            //    new Choice
            //    {
            //        OptionText = "<link>",
            //        IsCorrect = false,
            //        QuestionId = questions[3].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    },
            //    new Choice
            //    {
            //        OptionText = "Cascading Style Sheets",
            //        IsCorrect = true,
            //        QuestionId = questions[4].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new Choice
            //    {
            //        OptionText = "Computer Style Sheets",
            //        IsCorrect = false,
            //        QuestionId = questions[4].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new Choice
            //    {
            //        OptionText = "alert()",
            //        IsCorrect = true,
            //        QuestionId = questions[5].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new Choice
            //    {
            //        OptionText = "log()",
            //        IsCorrect = false,
            //        QuestionId = questions[5].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new Choice
            //    {
            //        OptionText = "Bootstrap 3",
            //        IsCorrect = true,
            //        QuestionId = questions[6].Id,
            //        CreatedDateUtc = DateTime.Now.AddMonths(-2)
            //    },
            //    new Choice
            //    {
            //        OptionText = "JavaScript Framework",
            //        IsCorrect = false,
            //        QuestionId = questions[6].Id,
            //        CreatedDateUtc = DateTime.Now.AddMonths(-2)
            //    }
            //};

            //            var badges = new List<Badge>
            //{
            //    new Badge
            //    {
            //        Name = "Outstanding Beginner",
            //        Description = "Awarded for completing the first course",
            //        ImageUrl = "badges/beginner.png",
            //        PointsRequired = 500,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-6)
            //    },
            //    new Badge
            //    {
            //        Name = "Golden Student",
            //        Description = "Awarded for achieving a score above 90% on tests",
            //        ImageUrl = "badges/golden.png",
            //        PointsRequired = 1000,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-4)
            //    },
            //    new Badge
            //    {
            //        Name = "Top Performer",
            //        Description = "Awarded for completing 5 courses with a score of 85% or higher",
            //        ImageUrl = "badges/top_performer.png",
            //        PointsRequired = 2500,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new Badge
            //    {
            //        Name = "Master Learner",
            //        Description = "Awarded for completing all available courses in a specialization",
            //        ImageUrl = "badges/master_learner.png",
            //        PointsRequired = 5000,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-1)
            //    },
            //    new Badge
            //    {
            //        Name = "Instructor Excellence",
            //        Description = "Awarded for having more than 10 students enrolled in courses",
            //        ImageUrl = "badges/instructor_excellence.png",
            //        PointsRequired = 1500,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    }
            //};

            //            var tickets = new List<Ticket>
            //{
            //    new Ticket
            //    {
            //        Title = "Problem playing video",
            //        Description = "Unable to watch the second video in Unit 1",
            //        Status = TicketStatusType.Opened, // الحالة مفتوحة
            //        CreateAt = DateTime.Now.AddDays(-5),
            //        ApplicationUserId = users[2].Id, // الطالب الثالث
            //        CreatedDateUtc = DateTime.UtcNow.AddDays(-5)
            //    },
            //    new Ticket
            //    {
            //        Title = "Unable to submit assignment",
            //        Description = "Assignment submission link not working for Unit 2",
            //        Status = TicketStatusType.Opened, // الحالة مفتوحة
            //        CreateAt = DateTime.Now.AddDays(-3),
            //        ApplicationUserId = users[1].Id, // المدرب الأول
            //        CreatedDateUtc = DateTime.UtcNow.AddDays(-3)
            //    }
            //};

            //            var ticketResponses = new List<TicketResponse>
            //{
            //    new TicketResponse
            //    {
            //        Message = "The problem has been resolved, please refresh the page and try again.",
            //        CreatedAt = DateTime.Now.AddDays(-4),
            //        ResponderUserId = users[0].Id, // المسؤول (Administrator)
            //        TicketId = tickets[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddDays(-4)
            //    },
            //    new TicketResponse
            //    {
            //        Message = "We are currently investigating the issue with the assignment submission.",
            //        CreatedAt = DateTime.Now.AddDays(-2),
            //        ResponderUserId = users[0].Id, // المسؤول (Administrator)
            //        TicketId = tickets[1].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddDays(-2)
            //    },
            //    new TicketResponse
            //    {
            //        Message = "The issue should be fixed now. Let us know if it persists.",
            //        CreatedAt = DateTime.Now.AddDays(-1),
            //        ResponderUserId = users[0].Id, // المسؤول (Administrator)
            //        TicketId = tickets[1].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddDays(-1)
            //    }
            //};

            //            var courseReviews = new List<CourseReview>
            //{
            //    new CourseReview
            //    {
            //        Rating = 5,
            //        Comment = "Great course, very clear explanation.",
            //        StudentId = students[0].Id,
            //        CourseId = courses[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddDays(-10)
            //    },
            //    new CourseReview
            //    {
            //        Rating = 4,
            //        Comment = "Good course but could use more examples.",
            //        StudentId = students[1].Id,
            //        CourseId = courses[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddDays(-7)
            //    },
            //    new CourseReview
            //    {
            //        Rating = 3,
            //        Comment = "Basic content. Expected more advanced topics.",
            //        StudentId = students[2].Id,
            //        CourseId = courses[1].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddDays(-5)
            //    }
            //};

            //            var learningPaths = new List<LearningPath>
            //{
            //new LearningPath
            //{
            //Name = "Full-stack Web Developer Path",
            //Description = "This path includes all web development courses from beginner to professional",
            //IconUrl = "paths/web.png",
            //CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //},
            //new LearningPath
            //{
            //Name = "C# Programmer Path",
            //Description = "Learn C# programming from the basics to advanced concepts",
            //IconUrl = "paths/csharp.png",
            //CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //},
            //new LearningPath
            //{
            //Name = "Computer Science Path",
            //Description = "Covering basic concepts in algorithms, data structures, and operating systems",
            //IconUrl = "paths/cs.png",
            //CreatedDateUtc = DateTime.UtcNow.AddMonths(-1)
            //}
            //};

            //            var courseLearningPaths = new List<CourseLearningPath>
            //{
            //    new CourseLearningPath
            //    {
            //        CourseId = courses[1].Id, // Web Development
            //        LearningPathId = learningPaths[0].Id, // مطور الويب
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new CourseLearningPath
            //    {
            //        CourseId = courses[0].Id, // C# Course
            //        LearningPathId = learningPaths[1].Id, // مبرمج C#
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new CourseLearningPath
            //    {
            //        CourseId = courses[0].Id, // C# Course
            //        LearningPathId = learningPaths[2].Id, // علوم الحاسوب
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-1)
            //    }
            //};

            //            var examAttempts = new List<UserExamAttemp>
            //{
            //    new UserExamAttemp
            //    {
            //        ExamScore = 100,
            //        UserScore = 85,
            //        StartedAt = DateTime.Now.AddDays(-3),
            //        FinishedAt = DateTime.Now.AddDays(-3).AddMinutes(25),
            //        StudentId = students[0].Id,
            //        ExamId = exams[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddDays(-3)
            //    },
            //    new UserExamAttemp
            //    {
            //        ExamScore = 100,
            //        UserScore = 92,
            //        StartedAt = DateTime.Now.AddDays(-2),
            //        FinishedAt = DateTime.Now.AddDays(-2).AddMinutes(18),
            //        StudentId = students[1].Id,
            //        ExamId = exams[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddDays(-2)
            //    },
            //    new UserExamAttemp
            //    {
            //        ExamScore = 100,
            //        UserScore = 60,
            //        StartedAt = DateTime.Now.AddDays(-1),
            //        FinishedAt = DateTime.Now.AddDays(-1).AddMinutes(30),
            //        StudentId = students[2].Id,
            //        ExamId = exams[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddDays(-1)
            //    }
            //};

            //            var userAnswers = new List<UserAnswer>
            //{
            //    new UserAnswer
            //    {
            //        UserExamAttempId = examAttempts[0].Id,
            //        QuestionId = questions[0].Id,
            //        ChoiceId = choices[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddDays(-3)
            //    },
            //    new UserAnswer
            //    {
            //        UserExamAttempId = examAttempts[0].Id,
            //        QuestionId = questions[1].Id,
            //        ChoiceId = choices[1].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddDays(-3)
            //    },
            //    new UserAnswer
            //    {
            //        UserExamAttempId = examAttempts[1].Id,
            //        QuestionId = questions[1].Id,
            //        ChoiceId = choices[1].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddDays(-2)
            //    }
            //};

            //            var nodeAttachments = new List<NodeAttachment>
            //{
            //    new NodeAttachment
            //    {
            //        Title = "PDF file for the explanation",
            //        Description = "You can download it for later reference",
            //        FileUrl = "attachments/lecture1.pdf",
            //        NodeId = nodes[1].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-4)
            //    },
            //    new NodeAttachment
            //    {
            //        Title = "Practice Code Files",
            //        Description = "ZIP archive with source code examples",
            //        FileUrl = "attachments/code-examples.zip",
            //        NodeId = nodes[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-5)
            //    },
            //    new NodeAttachment
            //    {
            //        Title = "Cheat Sheet",
            //        Description = "Quick reference for C# syntax",
            //        FileUrl = "attachments/cheatsheet.png",
            //        NodeId = nodes[2].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-5)
            //    }
            //};

            //            var watchedNodes = new List<UsersWatchedNode>
            //{
            //    new UsersWatchedNode
            //    {
            //        IsWatched = true,
            //        StudentId = students[0].Id,
            //        NodeId = nodes[0].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddDays(-5)
            //    },
            //    new UsersWatchedNode
            //    {
            //        IsWatched = false,
            //        StudentId = students[0].Id,
            //        NodeId = nodes[1].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddDays(-4)
            //    },
            //    new UsersWatchedNode
            //    {
            //        IsWatched = true,
            //        StudentId = students[1].Id,
            //        NodeId = nodes[2].Id,
            //        CreatedDateUtc = DateTime.UtcNow.AddDays(-2)
            //    }
            //};

            //            var certificate = new Certificate
            //            {
            //                StudentScore = "A+",
            //                EnrollmantCourseId = enrollments[0].Id,
            //                CreatedDateUtc = DateTime.UtcNow.AddDays(-2)
            //            };


            //            var userSessions = new List<UserSession>
            //{
            //    new UserSession
            //    {
            //        SessionId = Guid.NewGuid().ToString(),
            //        UserId = users[2].Id, 
            //        LoginTime = DateTime.Now.AddHours(-2),
            //        IpAddress = "192.168.1.100",
            //        DeviceInfo = "Chrome/Windows 10",
            //        IsActive = true
            //    },
            //    new UserSession
            //    {
            //        SessionId = Guid.NewGuid().ToString(),
            //        UserId = users[1].Id, 
            //        LoginTime = DateTime.Now.AddHours(-5),
            //        IpAddress = "192.168.1.101",
            //        DeviceInfo = "Firefox/macOS",
            //        IsActive = false
            //    },
            //    new UserSession
            //    {
            //        SessionId = Guid.NewGuid().ToString(),
            //        UserId = users[0].Id, 
            //        LoginTime = DateTime.Now.AddDays(-1),
            //        IpAddress = "192.168.1.102",
            //        DeviceInfo = "Edge/Windows 11",
            //        IsActive = true
            //    }
            //};


            //            var usersBadges = new List<UsersBadge>
            //{
            //    new UsersBadge
            //    {
            //        DateEarned = DateTime.Now.AddMonths(-1),
            //        StudentId = students[0].Id, // الطالب الأول
            //        BadgeId = badges[0].Id.ToString(), // الشارة الأولى (Outstanding Beginner)
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-1)
            //    },
            //    new UsersBadge
            //    {
            //        DateEarned = DateTime.Now.AddMonths(-2),
            //        StudentId = students[1].Id, // الطالب الثاني
            //        BadgeId = badges[1].Id.ToString(), // الشارة الثانية (Golden Student)
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-2)
            //    },
            //    new UsersBadge
            //    {
            //        DateEarned = DateTime.Now.AddMonths(-3),
            //        StudentId = students[2].Id, // الطالب الثالث
            //        BadgeId = badges[2].Id.ToString(), // الشارة الثالثة (Top Performer)
            //        CreatedDateUtc = DateTime.UtcNow.AddMonths(-3)
            //    }
            //};

            #endregion



        }

    }
}
