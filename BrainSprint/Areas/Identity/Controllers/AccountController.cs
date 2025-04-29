using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models.Enums;
using Models.ViewModels;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;


namespace BrainSprint.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AccountController(UserManager<ApplicationUser> userManager,
                                    SignInManager<ApplicationUser> signInManager,
                                        RoleManager<IdentityRole> roleManager,
                                            IEmailSender emailSender,
                                                IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;
        }


        #region Register

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            //  Check if roles do not exist, and create them if necessary
            if (_roleManager.Roles.IsNullOrEmpty())
            {
                await _roleManager.CreateAsync(role: new IdentityRole("SuperAdmin"));
                await _roleManager.CreateAsync(role: new IdentityRole("Admin"));
                await _roleManager.CreateAsync(role: new IdentityRole("Customer"));
                await _roleManager.CreateAsync(role: new IdentityRole("Instructor"));

            }

            if (User.Identity?.IsAuthenticated == true)
            {
                TempData["notifiction"] = "Your account has been created! Please check your email to confirm the account before logging in";
                TempData["MessageType"] = "Information";

                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
            return View(new RegisterVM()); //  Display the registration page for the user
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser
                {
                    FirstName = registerVM.FirstName,
                    LastName = registerVM.LastName,
                    Address = registerVM.Address,
                    UserName = registerVM.UserName,
                    Email = registerVM.Email,
                    RegistrationDate = DateTime.UtcNow,
                    IsActive = true,
                    Level = 1,
                    TotalPoints = 0,
                    EmailConfirmed = false
                };

                var newUser = await _userManager.CreateAsync(applicationUser, registerVM.Password);

                if (newUser.Succeeded)
                {
                    var userId = applicationUser.Id;
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                    var returnUrl = Url.Content("~/");

                    var callbackUrl = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                    protocol: Request.Scheme
                    );

                    await _emailSender.SendEmailAsync(registerVM.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");


                    TempData["notification"] = "Your account has been created! Please check your email to confirm the account before logging in";
                    TempData["MessageType"] = "Success";

                    await _userManager.AddToRoleAsync(applicationUser, "Customer");

                    if (applicationUser.EmailConfirmed)
                    {
                        await _signInManager.SignInAsync(applicationUser, isPersistent: false);
                        return RedirectToAction("Index", "Home", new { area = "Customer" });
                    }

                    return RedirectToAction("Login", "Account", new { area = "Identity" });
                }
                else
                {
                    foreach (var error in newUser.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }


            return View(registerVM);
        }

        #endregion


        #region LogOut

        // Log the user out
        public async Task<IActionResult> Logout()
        {
            // Perform the logout operation
            await _signInManager.SignOutAsync();

            // Redirect the user to the login page
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }

        #endregion


        #region Login

        // Display the login page when requested via HTTP GET
        [HttpGet]

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }

            // Display the login interface

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginVM.Email);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(loginVM);
                }

                if (user.IsBlocked)
                {
                    ModelState.AddModelError(string.Empty, "Account blocked. Contact support.");
                    return View(loginVM);
                }

                var result = await _signInManager.PasswordSignInAsync(
                    user.UserName!,
                    loginVM.Password,
                    loginVM.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // Debugging: Log the roles
                    var roles = await _userManager.GetRolesAsync(user);

                    if (await _userManager.IsInRoleAsync(user, "Admin") ||
                        await _userManager.IsInRoleAsync(user, "SuperAdmin"))
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    else if (await _userManager.IsInRoleAsync(user, "Instructor"))
                    {
                        return RedirectToAction("Dashboard", "Home", new { area = "Instructor" });
                    }
                    return RedirectToAction("Dashboard", "Home", new { area = "Customer" });
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(loginVM);
        }
        #endregion



        #region Forget Password

        // Display the "Forgot Password" page (GET)
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        // Receive form from "Forgot Password" page (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM model)
        {
            // Check the validity of the entered data
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Find user using email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Not disclosing that the mail is not registered for security reasons
                TempData["notification"] = "If your email is registered, you'll receive a password reset link";
                TempData["MessageType"] = "info";
                return RedirectToAction("ForgetPasswordConfirmation");
            }

            // Generate password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Create a password reset link
            var callbackUrl = Url.Action("ResetPassword", "Account",
                new { email = model.Email, token = token }, protocol: HttpContext.Request.Scheme);

            // Send an email containing a password reset link
            await _emailSender.SendEmailAsync(
                model.Email,
                "Reset your MovieMart password",
                $"Please reset your password by <a href='{callbackUrl}'>clicking here</a>.");

            // Notify the user that the link has been sent
            TempData["notification"] = "Password reset link has been sent to your email";
            TempData["MessageType"] = "success";
            return RedirectToAction("ForgetPasswordConfirmation");
        }


        // Display the confirmation page for sending the reset link (GET)
        [HttpGet]
        public IActionResult ForgetPasswordConfirmation()
        {
            return View();
        }

        // Display the password reset form (GET) based on the sent link
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            // Check if the code and email are present
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }

            // Set up the form with the postal and code values
            var forgetPassword = new ForgetPasswordVM
            {
                Email = email,
                ResetToken = token
            };

            return View(forgetPassword);
        }

        // Receive password reset form (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ForgetPasswordVM model)
        {
            // Data validation
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Find user using email
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                // Do not detect that the user does not exist
                TempData["notification"] = "Password has been reset successfully";
                TempData["MessageType"] = "success";
                return RedirectToAction("ResetPasswordConfirmation");
            }

            // Perform password reset operation using token
            var result = await _userManager.ResetPasswordAsync(user, model.ResetToken, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["notification"] = "Password has been reset successfully";
                TempData["MessageType"] = "success";
                return RedirectToAction("ResetPasswordConfirmation");
            }

            // If there are errors, display them to the user.
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // Display the password reset success confirmation page (GET)
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        #endregion





        #region ConfirmEmail (Beginning of the email confirmation section )

        // An asynchronous (Async) function used to confirm a user's email
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            // Checks that the entered values ​​are correct (userId and code must not be empty)
            if (userId == null || code == null)
            {
                return NotFound("Invalid email confirmation request."); // If the data is invalid, a 404 error is returned with a message explaining the problem
            }

            // Find the user in the database using their userId
            var user = await _userManager.FindByIdAsync(userId);

            // Checks if the user does not exist
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'."); // Return a 404 error if the user is not found
            }

            // Perform the email confirmation process using code
            var result = await _userManager.ConfirmEmailAsync(user, code);

            // Check if the confirmation process was successful
            if (result.Succeeded)
            {
                // Automatically log the user in after the email confirmation process is successful
                await _signInManager.SignInAsync(user, isPersistent: false);

                // Store a notification in TempData to display to the user in the interface
                TempData["notification"] = "Your email has been successfully confirmed! You have been automatically logged in.";
                TempData["MessageType"] = "Success"; // Specify the message type as success

                // Redirect the user to the profile page within the "Identity" area
                return RedirectToAction("Profile", "Settings", new { area = "Identity" });
            }

            // If the confirmation process fails, an error page is displayed.
            return View("Error");
        }
        #endregion // End of the email confirmation section


    }

}



