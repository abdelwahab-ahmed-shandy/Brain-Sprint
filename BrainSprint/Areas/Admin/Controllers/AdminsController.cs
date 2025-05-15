using Microsoft.AspNetCore.Mvc;

namespace BrainSprint.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ("SuperAdmin"))]
    public class AdminsController : Controller
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AdminsController> _logger;
        private readonly IAdminRepository _adminRepository;

        public AdminsController(IApplicationUserRepository applicationUserRepository, UserManager<ApplicationUser> userManager,
                                    ILogger<AdminsController> logger, IAdminRepository adminRepository)
        {
            _applicationUserRepository = applicationUserRepository;
            _userManager = userManager;
            _logger = logger;
            _adminRepository = adminRepository;
        }



        #region View Admin

        public async Task<IActionResult> Index(string? query, string? status, int page = 1)
        {
            var applicationUsers = await _applicationUserRepository.Get(tracked: false).ToListAsync();
            var adminVMs = new List<UserVM>();

            foreach (var user in applicationUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Admin"))
                {
                    adminVMs.Add(new UserVM
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
                adminVMs = adminVMs.Where(u =>
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
                adminVMs = adminVMs
                    .Where(u => u.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Create pagination info
            var pagination = new PaginationVM
            {
                CurrentPage = page,
                PageSize = 5,
                TotalItems = adminVMs.Count,
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
            var paginatedAdmins = adminVMs
                .Skip((page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            return View(new AdminListVM
            {
                Admins = paginatedAdmins,
                Pagination = pagination
            });
        }

        #endregion



        #region New Admin

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
                        await _userManager.AddToRoleAsync(user, "Admin");

                        await _adminRepository.CreateAsync(new Models.Admin()
                        {
                            ApplicationUserId = user.Id,
                            CreatedDateUtc = DateTime.UtcNow,
                            CurrentState = CurrentState.Active,
                            CreatedBy = User.Identity.Name
                        });

                        await _adminRepository.SaveInDataBaseAsync();


                        TempData["notification"] = "Admin created successfully!";
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
                    TempData["notification"] = "An error occurred while creating the Admin";
                    TempData["MessageType"] = "error";
                }
            }
            return View(model);
        }



        #endregion



        #region Details  Admin

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
            var AdminVM = new UserVM
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
            };
            return View(AdminVM);
        }

        #endregion



        #region Block Admin Account :

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
                userDB.BlockedDateUtc = DateTime.Now;
                userDB.BlockReason = "Account blocked by admin";
                userDB.IsBlocked = true;
                userDB.AccountState = AccountStateType.Blocked;
                userDB.IsActive = false;

                var result = await _userManager.UpdateAsync(userDB);

                if (result.Succeeded)
                {
                    var admin = await _adminRepository.GetOneAsync(i => i.ApplicationUserId == userDB.Id);

                    if (admin != null)
                    {
                        admin.BlockedBy = $"Block With {User.Identity.Name}";

                        await _adminRepository.EditAsync(admin);
                        await _adminRepository.SaveInDataBaseAsync();
                    }


                    TempData["notification"] = "The Admin's account has been successfully banned.";
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



        #region Un Block Admin Account :
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
                    var admin = await _adminRepository.GetOneAsync(i => i.ApplicationUserId == userDB.Id);

                    if (admin != null)
                    {
                        admin.BlockedBy = $"Un Block With {User.Identity.Name}";

                        await _adminRepository.EditAsync(admin);
                        await _adminRepository.SaveInDataBaseAsync();
                    }


                    TempData["notification"] = "The SuberAdmins's account has been successfully unblocked.";
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



        #region Delete Admin

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
                        .Include(u => u.Admin)
                        .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    TempData["Notification"] = "User not found";
                    TempData["MessageType"] = "error";
                    return RedirectToAction(nameof(Index));
                }

                if (user.Admin == null)
                {
                    TempData["Notification"] = "No admin record found for this user";
                    TempData["MessageType"] = "error";
                    return RedirectToAction(nameof(Index));
                }


                await _adminRepository.DeleteAsync(user.Admin);
                await _adminRepository.SaveInDataBaseAsync();


                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    TempData["Notification"] = "Account deleted successfully";
                    TempData["MessageType"] = "success";

                    return RedirectToAction(nameof(Index));
                }

                TempData["Notification"] = "Error deleting account";
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



        #region Edit Admin

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

            var admin = await _adminRepository.GetOneAsync(i => i.ApplicationUserId == user.Id);

            var AdminVM = new UserEditVM
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                IsActive = user.IsActive,
                AccountState = user.AccountState ?? AccountStateType.Active
            };
            return View(AdminVM);
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
                    var admin = await _adminRepository.GetOneAsync(i => i.ApplicationUserId == user.Id);

                    if (admin != null)
                    {
                        admin.UpdatedBy = User.Identity.Name;
                        admin.UpdatedDateUtc = DateTime.UtcNow;
                        admin.UpdatedBy = User.Identity.Name;
                        admin.CurrentState = model.CurrentState;


                        await _adminRepository.EditAsync(admin);
                        await _adminRepository.SaveInDataBaseAsync();
                    }

                    TempData["notification"] = "Admin updated successfully!";
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
