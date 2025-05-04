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
        private readonly IInstructorRepository _instructorRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ILogger<AccountController> _logger;
        public AccountController(UserManager<ApplicationUser> userManager,
                                   IInstructorRepository instructorRepository,
                                   IStudentRepository studentRepository,
                                    SignInManager<ApplicationUser> signInManager,
                                        RoleManager<IdentityRole> roleManager,
                                            IEmailSender emailSender,
                                                IWebHostEnvironment webHostEnvironment,
                                                ILogger<AccountController> logger)
        {
            _instructorRepository = instructorRepository;
            _studentRepository = studentRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }



        #region Register


        [HttpGet]
        public async Task<IActionResult> Register()
        {


            if (!await _roleManager.RoleExistsAsync("SuperAdmin"))
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));

            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await _roleManager.RoleExistsAsync("Student"))
                await _roleManager.CreateAsync(new IdentityRole("Student"));

            if (!await _roleManager.RoleExistsAsync("Instructor"))
                await _roleManager.CreateAsync(new IdentityRole("Instructor"));


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
        public async Task<IActionResult> Register(RegisterVM registerVM, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (registerVM.UserType == UserType.Instructor)
            {
                if (string.IsNullOrWhiteSpace(registerVM.Certifications))
                    ModelState.AddModelError("Certifications", "Certifications are required");

                if (string.IsNullOrWhiteSpace(registerVM.ExperienceYears))
                    ModelState.AddModelError("ExperienceYears", "Experience is required");
            }

            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            var applicationUser = new ApplicationUser
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
                EmailConfirmed = false,
                AccountState = AccountStateType.PendingActivation,
                Certifications = registerVM.Certifications,
                ExperienceYears = registerVM.ExperienceYears,

            };

            var newUser = await _userManager.CreateAsync(applicationUser, registerVM.Password);

            if (newUser.Succeeded)
            {
                string userRole = Enum.GetName(typeof(UserType), registerVM.UserType) ?? "Student";

                if (!await _roleManager.RoleExistsAsync(userRole))
                {
                    await _userManager.DeleteAsync(applicationUser);
                    ModelState.AddModelError("", "Invalid user role");
                    return View(registerVM);
                }

                await _userManager.AddToRoleAsync(applicationUser, userRole);

                try
                {
                    if (registerVM.UserType == UserType.Instructor)
                    {
                        await _instructorRepository.CreateAsync(new Models.Instructor()
                        {
                            ApplicationUserId = applicationUser.Id,
                            IsVerified = false,
                            CurrentState = CurrentState.Active,
                        });
                    }
                    else
                    {
                        await _studentRepository.CreateAsync(new Models.Student
                        {
                            ApplicationUserId = applicationUser.Id,
                            Level = LevelType.Beginner,
                            CurrentState = CurrentState.Active
                        });
                    }

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { area = "Identity", userId = applicationUser.Id, code, returnUrl },
                        protocol: Request.Scheme
                    );

                    try
                    {
                        await _emailSender.SendEmailAsync(
                            registerVM.Email,
                            "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error sending confirmation email");
                    }

                    TempData["notification"] = "Registration successful! Please check your email.";
                    TempData["MessageType"] = "success";
                    return RedirectToAction("Login", "Account", new { area = "Identity", returnUrl });
                }
                catch (Exception ex)
                {
                    await _userManager.DeleteAsync(applicationUser);
                    _logger.LogError(ex, "Error during registration");
                    ModelState.AddModelError("", "An error occurred during registration");
                    return View(registerVM);
                }
            }

            foreach (var error in newUser.Errors)
            {
                ModelState.AddModelError("", error.Description);
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
                    user.LastLogin = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);

                    var roles = await _userManager.GetRolesAsync(user);

                    if (await _userManager.IsInRoleAsync(user, "SuperAdmin") ||
                        await _userManager.IsInRoleAsync(user, "Admin"))
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
                "Reset your BrainSprint password",
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



        #region External Login

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null, UserType userType = UserType.Student)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { ReturnUrl = returnUrl });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            properties.Parameters.Add("userType", userType);

            properties.Items["userType"] = ((int)userType).ToString();

            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null, string userType = "0")
        {
            returnUrl ??= Url.Content("~/");

            if (!string.IsNullOrEmpty(remoteError))
            {
                TempData["notification"] = $"Service provider error: {remoteError}";
                TempData["MessageType"] = "error";
                return RedirectToAction(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["notification"] = "Failed to retrieve login information from Google.";
                TempData["MessageType"] = "error";
                return RedirectToAction(nameof(Login));
            }

            var userTypeValue = Request.Query["userType"].FirstOrDefault()
                                 ?? info.AuthenticationProperties?.Items["userType"]
                                 ?? userType;

            UserType parsedUserType;
            if (!Enum.TryParse(userTypeValue, out parsedUserType) &&
                !int.TryParse(userTypeValue, out int numericValue))
            {
                parsedUserType = UserType.Student;
                _logger.LogWarning($"Invalid userType value: {userTypeValue}, defaulting to Student");
            }
            else if (int.TryParse(userTypeValue, out numericValue))
            {
                parsedUserType = (UserType)numericValue;
            }

            string roleName = parsedUserType.ToString();
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            if (email == null)
            {
                TempData["notification"] = "Email not retrieved from Google account.";
                TempData["MessageType"] = "error";
                return RedirectToAction(nameof(Login));
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                TempData["notification"] = "There is no account associated with this email address. Please register first.";
                TempData["MessageType"] = "error";
                return RedirectToAction(nameof(Register), new { userType = parsedUserType });
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var currentUserType = currentRoles.FirstOrDefault();

            if (currentUserType != roleName)
            {
                TempData["notification"] = $"This account is registered as {currentUserType}. Please login as {currentUserType}.";
                TempData["MessageType"] = "error";

                return currentUserType switch
                {
                    "Instructor" => RedirectToAction("Login", "Account", new { userType = UserType.Instructor }),
                    _ => RedirectToAction("Login", "Account", new { userType = UserType.Student })
                };
            }

            var addLoginResult = await _userManager.AddLoginAsync(user, info);
            if (addLoginResult.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                TempData["notification"] = "Login successful.";
                TempData["MessageType"] = "success";
                return LocalRedirect(returnUrl);
            }

            return HandleErrors(addLoginResult.Errors, "An error occurred while linking the Google account.");
        }


        private IActionResult HandleErrors(IEnumerable<IdentityError> errors, string defaultMessage)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            TempData["notification"] = defaultMessage;
            TempData["MessageType"] = "error";
            return RedirectToAction(nameof(Login));
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

                return RedirectToAction("Manage", "Settings", new { area = "Identity" });
            }

            return View("Error");
        }
        #endregion




    }

}



