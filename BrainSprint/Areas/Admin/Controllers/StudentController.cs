
namespace BrainSprint.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ("SuperAdmin,Admin"))]
    public class StudentController : Controller
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<StudentController> _logger;
        private readonly IStudentRepository _studentRepository;
        public StudentController(IApplicationUserRepository applicationUserRepository, UserManager<ApplicationUser> userManager,
                                   IStudentRepository studentRepository, ILogger<StudentController> logger)
        {
            _applicationUserRepository = applicationUserRepository;
            _userManager = userManager;
            _logger = logger;
            _studentRepository = studentRepository;
        }



        #region View Student

        public async Task<IActionResult> Index(string? query, string? status, int page = 1)
        {
            var applicationUsers = await _applicationUserRepository.Get(tracked: false).ToListAsync();
            var StudentVMs = new List<UserVM>();

            foreach (var user in applicationUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Student"))
                {
                    StudentVMs.Add(new UserVM
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
                StudentVMs = StudentVMs.Where(u =>
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
                StudentVMs = StudentVMs
                    .Where(u => u.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Create pagination info
            var pagination = new PaginationVM
            {
                CurrentPage = page,
                PageSize = 5,
                TotalItems = StudentVMs.Count,
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
            var paginatedStudents = StudentVMs
                .Skip((page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            return View(new StudentListVM
            {
                Students = paginatedStudents,
                Pagination = pagination
            });
        }

        #endregion



        #region New Students

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
                        await _userManager.AddToRoleAsync(user, "Student");

                        await _studentRepository.CreateAsync(new Student
                        {
                            ApplicationUserId = user.Id,
                            CreatedDateUtc = DateTime.UtcNow,
                            CreatedBy = User.Identity.Name,
                            CurrentState = CurrentState.Active,

                        });

                        await _studentRepository.SaveInDataBaseAsync();

                        TempData["notification"] = "Student created successfully!";
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
                    TempData["notification"] = "An error occurred while creating the Student";
                    TempData["MessageType"] = "error";
                }
            }
            return View(model);
        }



        #endregion



        #region Details Student

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
            var StudentVM = new UserVM
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
            return View(StudentVM);
        }

        #endregion



        #region Block Student Account :

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
                userDB.BlockReason = "Blocked by Admin";
                var result = await _userManager.UpdateAsync(userDB);

                if (result.Succeeded)
                {
                    var student = await _studentRepository.GetOneAsync(i => i.ApplicationUserId == userDB.Id);

                    if (student != null)
                    {
                        student.BlockedBy = $"Block With {User.Identity.Name}";

                        await _studentRepository.EditAsync(student);
                        await _studentRepository.SaveInDataBaseAsync();
                    }

                    TempData["notification"] = "The Student's account has been successfully banned.";
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



        #region Un Block Student Account :
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
                    var student = await _studentRepository.GetOneAsync(i => i.ApplicationUserId == userDB.Id);

                    if (student != null)
                    {
                        student.BlockedBy = $"Un Block With {User.Identity.Name}";

                        await _studentRepository.EditAsync(student);
                        await _studentRepository.SaveInDataBaseAsync();
                    }

                    TempData["notification"] = "The Students's account has been successfully unblocked.";
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



        #region Delete Student

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
                     .Include(u => u.Student)
                     .FirstOrDefaultAsync(u => u.Id == id);


                if (user == null)
                {
                    TempData["Notification"] = "User not found";
                    TempData["MessageType"] = "error";
                    return RedirectToAction(nameof(Index));
                }

                if (user.Student == null)
                {
                    TempData["Notification"] = "No student record found for this user";
                    TempData["MessageType"] = "error";
                    return RedirectToAction(nameof(Index));
                }


                await _studentRepository.DeleteAsync(user.Student);
                await _studentRepository.SaveInDataBaseAsync();




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



        #region Edit Student

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

            var student = await _studentRepository.GetOneAsync(i => i.ApplicationUserId == user.Id);

            var StudentVM = new UserEditVM
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                IsActive = user.IsActive,
                AccountState = user.AccountState ?? AccountStateType.Active,
                CurrentState = student?.CurrentState ?? CurrentState.Active

            };
            return View(StudentVM);
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
                    var student = await _studentRepository.GetOneAsync(i => i.ApplicationUserId == user.Id);

                    if (student != null)
                    {
                        student.UpdatedBy = User.Identity.Name;
                        student.UpdatedDateUtc = DateTime.UtcNow;
                        student.UpdatedBy = User.Identity.Name;
                        student.CurrentState = model.CurrentState;


                        await _studentRepository.EditAsync(student);
                        await _studentRepository.SaveInDataBaseAsync();
                    }

                    TempData["notification"] = "Student updated successfully!";
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
