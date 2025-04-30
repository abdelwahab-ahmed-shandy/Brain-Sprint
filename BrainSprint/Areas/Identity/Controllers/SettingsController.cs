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
        private readonly ICustomEmailSender _emailSender;
        public SettingsController(UserManager<ApplicationUser> userManager,
                                    RoleManager<IdentityRole> roleManager,
                                      SignInManager<ApplicationUser> signInManager,
                                         IApplicationUserRepository applicationUserRepository,
                                            ICustomEmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _applicationUserRepository = applicationUserRepository;
            _emailSender = emailSender;
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
                    LastLogin = user.LastLogin,
                },
                Manage = new SettingsVM.ManageSettings(),
                DeleteAccount = new SettingsVM.DeleteAccountSettings()
            };

            settingsVM.Profile.TwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);

            return View(settingsVM);
        }

        #endregion


        #region Update Profile

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(SettingsVM settingsVM)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View("Manage", settingsVM);
            //}
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = settingsVM.Profile.FirstName;
            user.LastName = settingsVM.Profile.LastName;
            user.Bio = settingsVM.Profile.Bio;
            user.PhoneNumber = settingsVM.Profile.PhoneNumber;

            if (settingsVM.Profile.ImageFile != null && settingsVM.Profile.ImageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(settingsVM.Profile.ImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("Profile.ImageFile", "Invalid file type. Only JPG, JPEG, and PNG files are allowed.");

                    return View("Manage", settingsVM);
                }

                var fileName = $"{user.Id}_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Assets", "Identity", "images", "UserPhoto", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await settingsVM.Profile.ImageFile.CopyToAsync(stream);
                }

                user.ProfileImage = $"/Assets/Identity/images/UserPhoto/{fileName}";
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["notification"] = "Your account has been created! Please check your email to confirm the account before logging in";
                TempData["MessageType"] = "Success";
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return RedirectToAction("Manage", "Settings");
        }


        #endregion


        #region Change Password

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(SettingsVM settingsVM)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View("Manage", model);
            //}

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ChangePasswordAsync(
                user,
                settingsVM.Manage.CurrentPassword,
                settingsVM.Manage.NewPassword
            );

            if (result.Succeeded)
            {
                await _emailSender.SendPasswordChangedNotificationAsync(user.Email);

                TempData["notification"] = "Your password has been changed successfully!";
                TempData["MessageType"] = "Success";
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                TempData["notification"] = "Failed to change password. Please check your current password.";
                TempData["MessageType"] = "Error";
            }

            return RedirectToAction("Manage");
        }

        #endregion


        #region Delete Account

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteAccount(SettingsVM model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View("Manage", model);
        //    }

        //    // تأكد من أن نص التأكيد مطابق
        //    if (model.DeleteAccount?.ConfirmDelete?.ToUpper() != "DELETE MY ACCOUNT")
        //    {
        //        ModelState.AddModelError("DeleteAccount.DeleteConfirmation", "Please type the exact phrase to confirm");
        //        return View("Manage", model);
        //    }

        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        TempData["notification"] = "User not found.";
        //        TempData["MessageType"] = "Error";
        //        return RedirectToAction("Index", "Home");
        //    }

        //    var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.DeleteAccount.Password);
        //    if (!isPasswordValid)
        //    {
        //        TempData["Error"] = "The password you entered is incorrect.";
        //        return RedirectToAction("Manage");
        //    }

        //    if (await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "SuperAdmin"))
        //    {
        //        TempData["notification"] = "Admins and SuperAdmins cannot delete their own accounts.";
        //        TempData["MessageType"] = "Error";
        //        return RedirectToAction("Profile");
        //    }

        //    await _userManager.UpdateSecurityStampAsync(user);

        //    var result = await _userManager.DeleteAsync(user);
        //    if (!result.Succeeded)
        //    {
        //        TempData["notification"] = "An error occurred while deleting the account.";
        //        TempData["MessageType"] = "Error";
        //        return RedirectToAction("Profile");
        //    }

        //    await _signInManager.SignOutAsync();
        //    TempData["notification"] = "Your account has been successfully deleted.";
        //    TempData["MessageType"] = "Warning";

        //    return RedirectToAction("Login", "Account", new { area = "Identity" });
        //}

        #endregion


        #region Two-Factor Authentication

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleTwoFactor(bool enable)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Additional verification before activation
            if (enable)
            {
                if (!user.EmailConfirmed)
                {
                    TempData["Notification"] = "Email confirmation is required to enable two-step verification";
                    TempData["MessageType"] = "Error";
                    return RedirectToAction(nameof(Manage));
                }

                if (string.IsNullOrEmpty(user.PhoneNumber) || !user.PhoneNumberConfirmed)
                {
                    TempData["Notification"] = "You must add and confirm your phone number first to enable two-step verification";
                    TempData["MessageType"] = "Error";
                    return RedirectToAction(nameof(Manage));
                }
            }

            var result = await _userManager.SetTwoFactorEnabledAsync(user, enable);

            if (result.Succeeded)
            {
                // Send a notification to the user
                if (enable)
                {
                    await _emailSender.SendEmailAsync(user.Email,
                    "Two-step verification enabled",
                    "Two-step verification has been successfully activated for your account.");
                }

                TempData["Notification"] = enable
                ? "Two-step verification has been successfully activated!"
                : "Two-step verification is disabled";
                TempData["AlertType"] = "success";
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                TempData["Notification"] = $"Failed to update settings: {errors}";
                TempData["AlertType"] = "Error";
            }

            return RedirectToAction(nameof(Manage));
        }
        #endregion


        //todo : Active Sessions

        //#region Active Sessions
        //[HttpGet]
        //public async Task<IActionResult> GetActiveSessions()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //   // var sessions = await _userService.GetActiveSessionsAsync(user.Id);
        //   // return PartialView("_ActiveSessionsPartial", sessions);
        //}

        //[HttpPost]
        //public async Task<IActionResult> TerminateOtherSessions()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    await _signInManager.SignOutAsync();
        //    await _signInManager.SignInAsync(user, isPersistent: true);

        //    TempData["Success"] = "All other sessions have been terminated successfully!";
        //    return RedirectToAction("Manage");
        //}

        //#endregion


    }
}
