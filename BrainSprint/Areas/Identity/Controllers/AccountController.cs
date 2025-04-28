using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models.Enums;
using Models.ViewModels;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;


namespace BrainSprint.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        public AccountController(UserManager<ApplicationUser> userManager,
                                    SignInManager<ApplicationUser> signInManager,
                                        RoleManager<IdentityRole> roleManager,
                                            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }







        #region Email Confirmation

        /// <summary>
        /// Asynchronous function to confirm user email
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="code">Confirmation Code</param>
        /// <returns>Result</returns>
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            // Input validation
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                return View("ConfirmationError", new ConfirmationVM
                {
                    Title = "Invalid Email Confirmation Request",
                    Message = "Sorry, the confirmation link appears to be invalid or missing some information.",
                    Icon = "fas fa-exclamation-circle text-danger",
                    ButtonText = "Return to Home",
                    ButtonUrl = Url.Action("Index", "Home")
                });
            }

            // Find the user in the database
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return View("ConfirmationError", new ConfirmationVM
                {
                    Title = "User does not exist",
                    // Here we use a real string interpolation with $ before the text
                    Message = $"Sorry, we couldn't find an account associated with the ID '{userId}'.",
                    Icon = "fas fa-user-times text-warning",
                    ButtonText = "Attempting to log in",
                    ButtonUrl = Url.Action("Login", "Account")
                });
            }

            // Perform the email confirmation operation
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                // Automatic login after successful confirmation

                await _signInManager.SignInAsync(user, isPersistent: false);

                TempData["Notification"] = new NotificationVM

                {
                    Title = "Your email has been successfully confirmed!",
                    Message = "Thank you for confirming your email. You have been automatically logged in.",
                    Type = NotificationType.Success,
                    Icon = "fas fa-check-circle",
                    AutoDismiss = true,
                    Delay = 5000

                };

                return RedirectToAction("Profile", "Settings", new { area = "Identity" });
            }

            // If confirmation fails
            return View("ConfirmationError", new ConfirmationVM
            {
                Title = "Email confirmation failed",
                Message = "Sorry, an error occurred while trying to confirm your email. Please confirm the link or request a new confirmation link.",
                Icon = "fas fa-times-circle text-danger",
                ButtonText = "Request a new confirmation link",
                ButtonUrl = Url.Action("ResendConfirmationEmail", "Account")
            });
        }

        #endregion

    }
}
