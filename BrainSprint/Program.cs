
namespace BrainSprint
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add MVC services to the application, allowing the use of the Model-View-Controller architecture
            builder.Services.AddControllersWithViews();


            #region Register ApplicationDbContext with Dependency Injection 
            // Configured to use SQL Server with the connection string from app settings.
            builder.Services.AddDbContext<ApplicationDbContext>(option =>
                        option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            #endregion


            #region Add identity services to the application
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                // Login settings:
                option.SignIn.RequireConfirmedEmail = true;
                option.SignIn.RequireConfirmedAccount = false;
                option.SignIn.RequireConfirmedPhoneNumber = false;
                option.Tokens.ChangePhoneNumberTokenProvider = "Phone";
                option.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;

                // Password requirements:
                option.Password.RequireUppercase = true;
                option.Password.RequireNonAlphanumeric = true;
                option.Password.RequiredLength = 8;

                // User requirements:
                option.User.RequireUniqueEmail = true;
            }
            )
            // Bind the identity to the database using Entity Framework
            .AddEntityFrameworkStores<ApplicationDbContext>()
            // Add default token providers to support operations like password reset and email confirmation
            .AddDefaultTokenProviders();

            #endregion


            #region Email Sender 

            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.AddTransient<ICustomEmailSender, CustomEmailSender>();


            #endregion


            #region Register repository services with Dependency Injection (Scoped Lifetime) 

            // This ensures that a new instance is created per request, improving efficiency 
            // while maintaining consistency within a request's lifecycle.

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            builder.Services.AddScoped<IAdminRepository, AdminRepository>();
            builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();

            builder.Services.AddScoped<IActivityLogRepository, ActivityLogRepository>();

            builder.Services.AddScoped<IBadgeRepository, BadgeRepository>();
            builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<ICertificateRepository, CertificateRepository>();
            builder.Services.AddScoped<IChoiceRepository, ChoiceRepository>();
            builder.Services.AddScoped<ICourseLearningPathRepository, CourseLearningPathRepository>();
            builder.Services.AddScoped<ICourseModuleRepository, CourseModuleRepository>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<ICourseReviewRepository, CourseReviewRepository>();
            builder.Services.AddScoped<IEnrollmentCourseRepository, EnrollmentCourseRepository>();
            builder.Services.AddScoped<IExamRepository, ExamRepository>();
            builder.Services.AddScoped<ILearningPathRepository, LearningPathRepository>();
            builder.Services.AddScoped<INodeAttachmentRepository, NodeAttachmentRepository>();
            builder.Services.AddScoped<INodeRepository, NodeRepository>();
            builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<ITextNodeRepository, TextNodeRepository>();
            builder.Services.AddScoped<ITicketRepository, TicketRepository>();
            builder.Services.AddScoped<ITicketResponseRepository, TicketResponseRepository>();
            builder.Services.AddScoped<IUserAnswerRepository, UserAnswerRepository>();
            builder.Services.AddScoped<IUsersBadgeRepository, UsersBadgeRepository>();
            builder.Services.AddScoped<IUserSessionRepository, UserSessionRepository>();
            builder.Services.AddScoped<IUsersWatchedNodeRepository, UsersWatchedNodeRepository>();
            builder.Services.AddScoped<IVideoNodeRepository, VideoNodeRepository>();
            builder.Services.AddScoped<IUserExamAttempRepository, UserExamAttempRepository>();
            builder.Services.AddScoped<IEnrollmentCourseService, EnrollmentCourseService>();


            #endregion


            #region External Login

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;

                googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
            });

            #endregion


            #region Confige Stripe Setting

            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            #endregion



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Force HTTPS on all requests
            app.UseHttpsRedirection();

            app.UseRouting();


            #region Authentication & Authorization
            // Enable Authentication
            // Ensures that incoming requests pass user identity verification before accessing protected resources           
            app.UseAuthentication();

            // Enable Authorization
            // Determines whether the authenticated user has the required permissions to access certain resources           
            app.UseAuthorization();
            app.MapStaticAssets();

            #endregion


            app.MapStaticAssets();


            #region Setting up top-level routes

            // Enable routing requests to the appropriate paths
            app.UseRouting();

            // Route for the "Instructor" area
            app.MapControllerRoute(
            name: "Instructor",
            pattern: "{area:exists}/{controller=Home}/{action=Dashboard}/{id?}"
            );

            // Route for the "Admin" area
            app.MapControllerRoute(
            name: "Admin",
            pattern: "{area:exists}/{controller=Home}/{action=Dashboard}/{id?}"
            );

            // Route for the "Customer" area
            app.MapControllerRoute(
            name: "Customer",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );

            // Default Route
            // If no area is specified, the area will be assumed to be "Customer"
            app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}",
            defaults: new { area = "Customer" }
            );

            #endregion


            app.Run();
        }
    }
}
