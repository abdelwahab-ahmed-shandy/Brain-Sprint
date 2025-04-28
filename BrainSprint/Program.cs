using DataAccess.Repositories.IRepositories;
using DataAccess.Repositories;

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

            #endregion


            #region Configure Application Settings
            // Configure authentication services in the application

            //builder.Services.AddAuthentication(options =>
            //{
            //    // Specify the default authentication system using cookies
            //    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            //    // Specify Google as the authentication method when attempting to log in (when attempting to log in)
            //    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            //})
            //// Add authentication using cookies to store session data after login
            //.AddCookie()
            //// Add authentication via Google OAuth
            //.AddGoogle(googleOptions =>
            //{
            //    // Set the client ID for Google authentication from the application settings
            //    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];

            //    // Set the client secret key for Google authentication from the application settings 
            //    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            //});

            #endregion


            #region Register repository services with Dependency Injection (Scoped Lifetime) 
            // This ensures that a new instance is created per request, improving efficiency 
            // while maintaining consistency within a request's lifecycle.

            builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();

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


            #region Authentication & Authorization
            // Enable Authentication
            // Ensures that incoming requests pass user identity verification before accessing protected resources           
            app.UseAuthentication();

            // Enable Authorization
            // Determines whether the authenticated user has the required permissions to access certain resources           
            app.UseAuthorization();
            app.MapStaticAssets();

            #endregion


            #region Setting up top-level routes

            // Enable routing requests to the appropriate paths
            app.UseRouting();

            // Route for the "Instructor" area
            app.MapControllerRoute(
            name: "Instructor",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );

            // Route for the "Admin" area
            app.MapControllerRoute(
            name: "Admin",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
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
