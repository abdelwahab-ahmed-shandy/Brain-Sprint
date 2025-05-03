using DataAccess.Repositories;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Models;
using Models.ViewModels;
using System.Linq.Expressions;

namespace BrainSprint.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAdminRepository _adminRepository;
        private readonly IInstructorRepository _instructorRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly ICustomEmailSender _emailSender;
        private readonly ILogger<SettingsController> _logger;
        private readonly IEnrollmentCourseRepository _enrollmentCourseRepository;
        private readonly ICourseReviewRepository _courseReviewRepository;
        private readonly IUsersBadgeRepository _usersBadgeRepository;
        public SettingsController(UserManager<ApplicationUser> userManager,
                                    RoleManager<IdentityRole> roleManager,
                                      SignInManager<ApplicationUser> signInManager,
                                         IApplicationUserRepository applicationUserRepository,
                                            ICustomEmailSender emailSender,
                                            IAdminRepository adminRepository,
                                            IInstructorRepository instructorRepository,
                                            IStudentRepository studentRepository,
                                            ILogger<SettingsController> logger,
                                            IEnrollmentCourseRepository enrollmentCourseRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _applicationUserRepository = applicationUserRepository;
            _emailSender = emailSender;
            _adminRepository = adminRepository;
            _instructorRepository = instructorRepository;
            _studentRepository = studentRepository;
            _logger = logger;
            _enrollmentCourseRepository = enrollmentCourseRepository;
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

            var userRoles = await _userManager.GetRolesAsync(user);

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
                    TwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user),
                    Certifications = user.Certifications,
                    ExperienceYears = user.ExperienceYears
                },
                Manage = new SettingsVM.ManageSettings(),
                DeleteAccount = new SettingsVM.DeleteAccountSettings()
            };

            if (userRoles.Contains("Student"))
            {
                var student = await _studentRepository.GetOneAsync(
                    s => s.ApplicationUserId == user.Id,
                    includes: new List<Expression<Func<Student, object>>>
                    {
                        s => s.EnrollmentCourses,
                        s => s.UsersBadges
                    });

                if (student != null)
                {
                    settingsVM.Profile.StudentLevel = student.Level;
                    settingsVM.Profile.EnrolledCoursesCount = student.EnrollmentCourses?.Count ?? 0;
                    settingsVM.Profile.BadgesCount = student.UsersBadges?.Count ?? 0;
                }
            }

            if (userRoles.Contains("Instructor"))
            {
                var instructor = await _instructorRepository.GetOneAsync(
                    i => i.ApplicationUserId == user.Id,
                    includes: new List<Expression<Func<Models.Instructor, object>>>
                    {
                i => i.Courses
                    });

                if (instructor != null)
                {

                    settingsVM.Profile.InstructorRating = instructor.Rating;
                    settingsVM.Profile.IsVerified = instructor.IsVerified;
                    settingsVM.Profile.TotalStudents = instructor.Courses?.Sum(c => c.EnrollmentCourses?.Count ?? 0) ?? 0;
                }
            }

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

            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                user.FirstName = settingsVM.Profile.FirstName;
                user.LastName = settingsVM.Profile.LastName;
                user.Bio = settingsVM.Profile.Bio;
                user.PhoneNumber = settingsVM.Profile.PhoneNumber;
                user.Certifications = settingsVM.Profile.Certifications;
                user.ExperienceYears = settingsVM.Profile.ExperienceYears;

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
                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Assets", "Identity", "images", "UserPhoto");

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    var filePath = Path.Combine(uploadFolder, fileName);

                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await settingsVM.Profile.ImageFile.CopyToAsync(stream);
                        }

                        if (!string.IsNullOrWhiteSpace(user.ProfileImage))
                        {
                            var oldFileName = Path.GetFileName(user.ProfileImage);
                            var oldPath = Path.Combine(uploadFolder, oldFileName);

                            if (System.IO.File.Exists(oldPath))
                            {
                                System.IO.File.Delete(oldPath);
                            }
                        }

                        user.ProfileImage = $"/Assets/Identity/images/UserPhoto/{fileName}";
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Profile.ImageFile", $"File upload failed: {ex.Message}");
                        return View("Manage", settingsVM);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(settingsVM.Profile.ProfileImage))
                {
                    user.ProfileImage = settingsVM.Profile.ProfileImage;
                }


                if (await _userManager.IsInRoleAsync(user, "Student"))
                {
                    var student = await _studentRepository.GetOneAsync(s => s.ApplicationUserId == user.Id);

                    if (student == null)
                    {
                        student = new Models.Student
                        {
                            ApplicationUserId = user.Id,
                            Level = settingsVM.Profile.StudentLevel ?? LevelType.Beginner
                        };
                        await _studentRepository.CreateAsync(student);
                    }
                    else if (settingsVM?.Profile?.StudentLevel != null)
                    {
                        student.Level = settingsVM.Profile.StudentLevel.Value;
                        await _studentRepository.EditAsync(student);
                    }
                    await _studentRepository.SaveInDataBaseAsync();
                }

                else if (await _userManager.IsInRoleAsync(user, "Instructor"))
                {
                    var instructor = await _instructorRepository.GetOneAsync(i => i.ApplicationUserId == user.Id);

                    if (instructor == null)
                    {
                        instructor = new Models.Instructor
                        {
                            ApplicationUserId = user.Id,
                        };
                        await _instructorRepository.CreateAsync(instructor);
                    }
                    else
                    {
                        await _instructorRepository.EditAsync(instructor);
                    }
                    await _instructorRepository.SaveInDataBaseAsync();
                }


                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["notification"] = "Profile updated successfully!";
                    TempData["MessageType"] = "Success";
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View("Manage", settingsVM);
                }

                return RedirectToAction("Manage");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile");
                TempData["notification"] = "An error occurred while updating your profile.";
                TempData["MessageType"] = "Error";
                return View("Manage", settingsVM);
            }
        }

        #endregion


        #region Change Password

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(SettingsVM settingsVM)
        {

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount(SettingsVM model)
        {
            if (model.DeleteAccount?.DeleteConfirmation?.ToUpper() != "DELETE MY ACCOUNT")
            {
                ModelState.AddModelError("DeleteAccount.DeleteConfirmation", "Please type the exact phrase to confirm");
                return View("Manage", model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            await _userManager.UpdateSecurityStampAsync(user);

            if (!await _userManager.CheckPasswordAsync(user, model.DeleteAccount.Password))
            {
                ModelState.AddModelError("DeleteAccount.Password", "The password you entered is incorrect.");
                TempData["notification"] = "The password you entered is incorrect!";
                TempData["MessageType"] = "Error";
                return View("Manage", model);
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Any(r => new[] { "Admin", "SuperAdmin", "Instructor" }.Contains(r)))
            {
                ModelState.AddModelError("", "Admins and Instructors cannot delete their accounts.");
                TempData["notification"] = "Admins and Instructors cannot delete their accounts";
                TempData["MessageType"] = "Error";
                return View("Manage", model);
            }

            try
            {
                if (userRoles.Contains("Student"))
                {
                    var student = await _studentRepository.GetOneAsync(
                        s => s.ApplicationUserId == user.Id,
                        includes: new List<Expression<Func<Student, object>>>
                        {
                            s => s.EnrollmentCourses,
                            s => s.CourseReviews,
                            s => s.UsersBadges
                        });

                    if (student != null)
                    {
                        if (student.EnrollmentCourses?.Any() == true)
                        {
                            await _enrollmentCourseRepository.DeleteAllAsync(student.EnrollmentCourses);
                        }

                        if (student.CourseReviews?.Any() == true)
                        {
                            await _courseReviewRepository.DeleteAllAsync(student.CourseReviews);
                        }

                        if (student.UsersBadges?.Any() == true)
                        {
                            await _usersBadgeRepository.DeleteAllAsync(student.UsersBadges);
                        }

                        await _studentRepository.DeleteAsync(student);
                    }
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    _logger.LogError("Failed to delete user: {Errors}", string.Join(", ", result.Errors));
                    ModelState.AddModelError("", "Failed to delete account. Please try again.");
                    return View("Manage", model);
                }

                await _signInManager.SignOutAsync();

                TempData["notification"] = "Your account has been deleted successfully.";
                TempData["MessageType"] = "Success";
                return RedirectToAction("Login", "Identity", new { area = "Identity" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting account for user {UserId}", user.Id);
                ModelState.AddModelError("", "An error occurred while deleting your account.");
                return View("Manage", model);
            }
        }

        #endregion



        //==============================================================

        // todo : Two-Factor Authentication
        #region Two-Factor Authentication

        /*[HttpPost]
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
        }*/
        #endregion


        //todo : Active Sessions
        /*#region Active Sessions
        [HttpGet]
        public async Task<IActionResult> GetActiveSessions()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // var sessions = await _userService.GetActiveSessionsAsync(user.Id);
            // return PartialView("_ActiveSessionsPartial", sessions);
        }

        [HttpPost]
        public async Task<IActionResult> TerminateOtherSessions()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, isPersistent: true);

            TempData["Success"] = "All other sessions have been terminated successfully!";
            return RedirectToAction("Manage");
        }

        #endregion */


    }
}
