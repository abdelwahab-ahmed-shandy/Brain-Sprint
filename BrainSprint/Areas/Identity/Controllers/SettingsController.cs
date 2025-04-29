using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Models.ViewModels;

namespace BrainSprint.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IApplicationUserRepository _applicationUserRepository;

        public SettingsController(UserManager<ApplicationUser> userManager,
                                    RoleManager<IdentityRole> roleManager,
                                      SignInManager<ApplicationUser> signInManager,
                                         IApplicationUserRepository applicationUserRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _applicationUserRepository = applicationUserRepository;
        }

        #region Manage Profile 

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            ViewData["Title"] = "Profile";

            var settingsVM = new SettingsVM
            {
                Profile = new SettingsVM.ProfileSettings
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailVerified = user.EmailConfirmed,
                    ProfileImage = user.ProfileImage,
                    Bio = user.Bio,
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    Level = user.Level,
                    TotalPoints = user.TotalPoints,
                    RegistrationDate = user.RegistrationDate,
                    LastLogin = user.LastLogin
                },
                Manage = new SettingsVM.ManageSettings(),
                DeleteAccount = new SettingsVM.DeleteAccountSettings()
            };

            return View(settingsVM);
        }

        #endregion


        #region Update Profile

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UpdateProfile(SettingsVM settingsVM)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View("Manage", settingsVM);
        //    }

        //    var user = await _userManager.GetUserAsync(User);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    user.FirstName = settingsVM.Profile.FirstName;
        //    user.LastName = settingsVM.Profile.LastName;
        //    user.Bio = settingsVM.Profile.Bio;
        //    user.PhoneNumber = settingsVM.Profile.PhoneNumber;

        //    if(settingsVM.Profile.ImageFile != null && settingsVM.Profile.ImageFile.Length > 0)
        //    {
        //        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        //    }

        //}


        #endregion



    }
}
