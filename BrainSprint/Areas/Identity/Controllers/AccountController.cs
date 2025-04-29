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
            if (_roleManager.Roles.IsNullOrEmpty())
            {
                await _roleManager.CreateAsync(role: new IdentityRole("SuperAdmin"));
                await _roleManager.CreateAsync(role: new IdentityRole("Admin"));
                await _roleManager.CreateAsync(role: new IdentityRole("Customer"));
                await _roleManager.CreateAsync(role: new IdentityRole("Instructor"));

            }

            if (User.Identity?.IsAuthenticated == true)
            {
                TempData["notification"] = "Your account has been created! Please check your email to confirm the account before logging in";
                TempData["MessageType"] = "Information";

                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
            return View(new RegisterVM());
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

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }

        #endregion


        #region Login

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
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

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["notification"] = "If your email is registered, you'll receive a password reset link";
                TempData["MessageType"] = "info";
                return RedirectToAction("ForgetPasswordConfirmation");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = Url.Action("ResetPassword", "Account",
                new { email = model.Email, token = token }, protocol: HttpContext.Request.Scheme);

            await _emailSender.SendEmailAsync(
                model.Email,
                "Reset your MovieMart password",
                $"Please reset your password by <a href='{callbackUrl}'>clicking here</a>.");

            TempData["notification"] = "Password reset link has been sent to your email";
            TempData["MessageType"] = "success";
            return RedirectToAction("ForgetPasswordConfirmation");
        }


        [HttpGet]
        public IActionResult ForgetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }

            var forgetPassword = new ForgetPasswordVM
            {
                Email = email,
                ResetToken = token
            };

            return View(forgetPassword);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ForgetPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                TempData["notification"] = "Password has been reset successfully";
                TempData["MessageType"] = "success";
                return RedirectToAction("ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.ResetToken, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["notification"] = "Password has been reset successfully";
                TempData["MessageType"] = "success";
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        #endregion





        #region ConfirmEmail (Beginning of the email confirmation section )

        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return NotFound("Invalid email confirmation request.");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);

                TempData["notification"] = "Your email has been successfully confirmed! You have been automatically logged in.";
                TempData["MessageType"] = "Success";

                return RedirectToAction("Profile", "Settings", new { area = "Identity" });
            }

            return View("Error");
        }
        #endregion 


    }

}



