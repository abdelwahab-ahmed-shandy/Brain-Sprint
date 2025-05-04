using Microsoft.AspNetCore.Mvc;

namespace BrainSprint.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ("SuperAdmin"))]
    public class SuperAdminsController : Controller
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public SuperAdminsController(IApplicationUserRepository applicationUserRepository,
                                        UserManager<ApplicationUser> userManager)
        {
            _applicationUserRepository = applicationUserRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var applicationUsers = await _applicationUserRepository.Get(tracked: false).ToListAsync();

            var superAdminViewModels = new List<SuperAdminViewModel>();

            foreach (var user in applicationUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("SuperAdmin"))
                {
                    superAdminViewModels.Add(new SuperAdminViewModel
                    {
                        Id = user.Id,
                        FullName = $"{user.FirstName} {user.LastName}",
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        ProfileImage = user.ProfileImage,
                        Status = user.IsBlocked ? "Blocked" : (user.IsActive ? "Active" : "Inactive"),
                        Bio = user.Bio,
                        RegistrationDate = user.RegistrationDate,
                        LastLogin = user.LastLogin
                    });
                }
            }

            return View(superAdminViewModels);
        }



    }
}
