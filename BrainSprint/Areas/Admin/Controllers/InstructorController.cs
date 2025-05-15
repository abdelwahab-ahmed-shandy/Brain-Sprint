
namespace BrainSprint.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ("SuperAdmin,Admin"))]
    public class InstructorController : Controller
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IInstructorRepository _instructorRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<InstructorController> _logger;
        public InstructorController(IApplicationUserRepository applicationUserRepository, UserManager<ApplicationUser> userManager,
                                    ILogger<InstructorController> logger, IInstructorRepository instructorRepository)
        {
            _applicationUserRepository = applicationUserRepository;
            _userManager = userManager;
            _logger = logger;
            _instructorRepository = instructorRepository;
        }



        #region View Instructor

        public async Task<IActionResult> Index(string? query, string? status, int page = 1)
        {
            var applicationUsers = await _applicationUserRepository.Get(tracked: false).ToListAsync();
            var InstructorVMs = new List<UserVM>();

            foreach (var user in applicationUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Instructor"))
                {
                    InstructorVMs.Add(new UserVM
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        ProfileImage = user.ProfileImage,
                        Status = user.IsBlocked ? "Blocked" : (user.IsActive ? "Active" : "Inactive"),
                        Bio = user.Bio,
                        RegistrationDate = user.RegistrationDate,
                        LastLogin = user.LastLogin,
                        AccountState = user.AccountState ?? AccountStateType.Banned,
                    });
                }
            }

            // Apply filters
            if (!string.IsNullOrEmpty(query))
            {
                InstructorVMs = InstructorVMs.Where(u =>
                    (u.FirstName?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (u.LastName?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (u.Email?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (u.PhoneNumber?.Contains(query) == true) ||
                    (u.Status?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (u.Bio?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                     u.RegistrationDate.ToString("yyyy-MM-dd").Contains(query) ||
                    (u.LastLogin?.ToString("yyyy-MM-dd").Contains(query) == true) ||
                     u.AccountState.ToString().Contains(query, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            if (!string.IsNullOrEmpty(status))
            {
                InstructorVMs = InstructorVMs
                    .Where(u => u.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Create pagination info
            var pagination = new PaginationVM
            {
                CurrentPage = page,
                PageSize = 5,
                TotalItems = InstructorVMs.Count,
                Query = query,
                StatusFilter = status
            };

            // Validate page against total pages
            if (page > pagination.TotalPages && pagination.TotalPages > 0)
            {
                page = pagination.TotalPages;
                pagination.CurrentPage = page;
            }

            // Apply pagination
            var paginatedInstructors = InstructorVMs
                .Skip((page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            return View(new InstructorListVM
            {
                Instructors = paginatedInstructors,
                Pagination = pagination
            });
        }

        #endregion



        #region New Instructors

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new UserCreateVM
            {
                RegistrationDate = DateTime.Now,
                IsActive = true,
                AccountState = AccountStateType.Active
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Passwords do not match");
                    return View(model);
                }

                try
                {
                    var user = new ApplicationUser
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        UserName = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Address = model.Address,
                        IsActive = model.IsActive,
                        AccountState = model.AccountState,
                        RegistrationDate = DateTime.Now,
                        EmailConfirmed = true
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Instructor");

                        await _instructorRepository.CreateAsync(new Models.Instructor()
                        {
                            ApplicationUserId = user.Id,
                            IsVerified = true,
                            CurrentState = CurrentState.Active,
                            CreatedBy = User.Identity.Name,
                            CreatedDateUtc = DateTime.Now,
                        });

                        await _instructorRepository.SaveInDataBaseAsync();

                        TempData["notification"] = "Instructor created successfully!";
                        TempData["MessageType"] = "success";
                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (Exception ex)
                {
                    TempData["notification"] = "An error occurred while creating the Instructor";
                    TempData["MessageType"] = "error";
                }
            }
            return View(model);
        }



        #endregion



        #region Details Instructor

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["notification"] = "User not found";
                TempData["MessageType"] = "error";
                return RedirectToAction(nameof(Index));
            }
            var roles = await _userManager.GetRolesAsync(user);
            var InstructorVM = new UserVM
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ProfileImage = user.ProfileImage,
                Status = user.IsBlocked ? "Blocked" : (user.IsActive ? "Active" : "Inactive"),
                Bio = user.Bio,
                RegistrationDate = user.RegistrationDate,
                LastLogin = user.LastLogin,
                AccountState = user.AccountState ?? AccountStateType.Banned,
                Certifications = user.Certifications,
                ExperienceYears = user.ExperienceYears,
            };
            return View(InstructorVM);
        }

        #endregion



        #region Block Instructor Account :

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Block(string Id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
            {
                TempData["Notification"] = "User session invalid";
                TempData["MessageType"] = "error";

                return RedirectToAction(nameof(Index));
            }

            if (currentUserId == Id)
            {
                TempData["Notification"] = "You cannot Block your own account!";
                TempData["MessageType"] = "error";
                return RedirectToAction(nameof(Index));
            }

            var userDB = await _userManager.FindByIdAsync(Id);

            if (userDB != null)
            {

                userDB.IsBlocked = true;
                userDB.AccountState = AccountStateType.Blocked;
                userDB.IsActive = false;
                userDB.BlockedDateUtc = DateTime.UtcNow;
                userDB.BlockReason = "Account blocked by admin";

                var result = await _userManager.UpdateAsync(userDB);

                if (result.Succeeded)
                {
                    var instructor = await _instructorRepository.GetOneAsync(i => i.ApplicationUserId == userDB.Id);

                    if (instructor != null)
                    {
                        instructor.BlockedBy = $"Block With {User.Identity.Name}";

                        await _instructorRepository.EditAsync(instructor);
                        await _instructorRepository.SaveInDataBaseAsync();
                    }


                    TempData["notification"] = "The Instructor's account has been successfully banned.";
                    TempData["MessageType"] = "Warning";

                    _logger.LogInformation($"User {userDB.Email} has been blocked.");
                }
                else
                {
                    TempData["notification"] = "An error occurred while blocking the account.";
                    TempData["MessageType"] = "error";
                }

                return RedirectToAction(nameof(Index));
            }

            TempData["notification"] = "Client not found?!";
            TempData["MessageType"] = "error";

            return RedirectToAction(nameof(Index));
        }

        #endregion



        #region Un Block Instructor Account :
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnBlock(string Id)
        {
            var userDB = await _userManager.FindByIdAsync(Id);

            if (userDB != null)
            {
                userDB.IsBlocked = false;
                userDB.AccountState = AccountStateType.Active;
                userDB.IsActive = true;

                var result = await _userManager.UpdateAsync(user: userDB);

                if (result.Succeeded)
                {
                    var instructor = await _instructorRepository.GetOneAsync(i => i.ApplicationUserId == userDB.Id);

                    if (instructor != null)
                    {
                        instructor.BlockedBy = $"Un Block With {User.Identity.Name}";

                        await _instructorRepository.EditAsync(instructor);
                        await _instructorRepository.SaveInDataBaseAsync();
                    }

                    TempData["notification"] = "The Instructors's account has been successfully unblocked.";
                    TempData["MessageType"] = "Success";

                    _logger.LogInformation($"User {userDB.Email} has been unblocked.");
                }
                else
                {
                    TempData["notification"] = "An error occurred while unblocking the account.";
                    TempData["MessageType"] = "error";
                }
                return RedirectToAction(nameof(Index));
            }
            TempData["notification"] = "Client not found?!";
            TempData["MessageType"] = "error";

            return RedirectToAction(nameof(Index));
        }

        #endregion



        #region Delete Instructor

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Challenge();
                }

                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(currentUserId))
                {
                    TempData["Notification"] = "User session invalid";
                    TempData["MessageType"] = "error";

                    return RedirectToAction(nameof(Index));
                }

                if (currentUserId == id)
                {
                    TempData["Notification"] = "You cannot delete your own account!";
                    TempData["MessageType"] = "error";
                    return RedirectToAction(nameof(Index));
                }


                var user = await _userManager.Users
                    .Include(u => u.Instructor)
                    .FirstOrDefaultAsync(u => u.Id == id);


                if (user == null)
                {
                    TempData["Notification"] = "User not found";
                    TempData["MessageType"] = "error";
                    return RedirectToAction(nameof(Index));
                }

                if (user.Instructor == null)
                {
                    TempData["Notification"] = "No instructor record found for this user";
                    TempData["MessageType"] = "error";
                    return RedirectToAction(nameof(Index));
                }


                await _instructorRepository.DeleteAsync(user.Instructor);
                await _instructorRepository.SaveInDataBaseAsync();

                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {

                    TempData["Notification"] = "Instructor account deleted successfully";
                    TempData["MessageType"] = "success";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Notification"] = "Error deleted user account";
                TempData["MessageType"] = "error";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                TempData["Notification"] = "Error deleting account";
                TempData["MessageType"] = "error";
                return RedirectToAction(nameof(Index));
            }
        }


        #endregion



        #region Reset Password

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string id, string newPassword, string confirmPassword, bool forceChange = true)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newPassword) || newPassword != confirmPassword)
                {
                    TempData["Notification"] = "Passwords do not match or are empty";
                    return RedirectToAction(nameof(Index));
                }

                var currentUser = await _userManager.GetUserAsync(User);
                var targetUser = await _userManager.FindByIdAsync(id);

                if (targetUser == null)
                {
                    TempData["Notification"] = "User not found";
                    return RedirectToAction(nameof(Index));
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(targetUser);
                var resetResult = await _userManager.ResetPasswordAsync(targetUser, token, newPassword);

                if (resetResult.Succeeded)
                {
                    // Track password change
                    targetUser.PasswordChangedDate = DateTime.UtcNow;
                    await _userManager.UpdateAsync(targetUser);

                    if (forceChange)
                    {
                        await _userManager.ResetAuthenticatorKeyAsync(targetUser);
                    }

                    TempData["Notification"] = "Password reset successfully";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Notification"] = string.Join(", ", resetResult.Errors.Select(e => e.Description));
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password");
                TempData["Notification"] = "Error resetting password";
                return RedirectToAction(nameof(Index));
            }
        }


        #endregion



        #region Edit Instructor

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["notification"] = "User not found";
                TempData["MessageType"] = "error";
                return RedirectToAction(nameof(Index));
            }

            var instructor = await _instructorRepository.GetOneAsync(i => i.ApplicationUserId == user.Id);

            var InstructorVM = new UserEditVM
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                IsActive = user.IsActive,
                AccountState = user.AccountState ?? AccountStateType.Active,
                CurrentState = instructor?.CurrentState ?? CurrentState.Active,

            };
            return View(InstructorVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    TempData["notification"] = "User not found";
                    TempData["MessageType"] = "error";
                    return RedirectToAction(nameof(Index));
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                user.Address = model.Address;
                user.IsActive = model.IsActive;
                user.AccountState = model.AccountState;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var instructor = await _instructorRepository.GetOneAsync(i => i.ApplicationUserId == user.Id);

                    if (instructor != null)
                    {
                        instructor.UpdatedBy = User.Identity.Name;
                        instructor.UpdatedDateUtc = DateTime.UtcNow;
                        instructor.UpdatedBy = User.Identity.Name;
                        instructor.CurrentState = model.CurrentState;


                        await _instructorRepository.EditAsync(instructor);
                        await _instructorRepository.SaveInDataBaseAsync();
                    }

                    TempData["notification"] = "Instructor updated successfully!";
                    TempData["MessageType"] = "success";
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
            return View(model);
        }



        #endregion


    }
}
