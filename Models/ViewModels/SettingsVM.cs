using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace Models.ViewModels
{
    public class SettingsVM
    {
        public ProfileSettings Profile { get; set; } = new ProfileSettings();
        public ManageSettings Manage { get; set; } = new ManageSettings();
        public DeleteAccountSettings DeleteAccount { get; set; } = new DeleteAccountSettings();

        public class ProfileSettings
        {
            [Display(Name = "First Name")]
            [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
            public string? FirstName { get; set; }

            [Display(Name = "Last Name")]
            [MaxLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
            public string? LastName { get; set; }

            [Display(Name = "Full Name")]
            public string FullName => $"{FirstName} {LastName}".Trim();

            [Required]
            [EmailAddress]
            [Display(Name = "Email Address")]
            public string Email { get; set; } = string.Empty;

            [Display(Name = "Email Verified")]
            public bool EmailVerified { get; set; }

            [Display(Name = "Profile Image")]
            [RegularExpression(@"^.*\.(jpg|jpeg|png)$", ErrorMessage = "Only JPG, JPEG, and PNG files are allowed.")]
            public string? ProfileImage { get; set; }

            [Display(Name = "Upload New Image")]
            [DataType(DataType.Upload)]
            public IFormFile? ImageFile { get; set; }

            [Display(Name = "Biography")]
            [MaxLength(1000, ErrorMessage = "Bio cannot exceed 1000 characters.")]
            public string? Bio { get; set; }

            [Phone]
            [Display(Name = "Phone Number")]
            public string? PhoneNumber { get; set; }

            [Display(Name = "Phone Verified")]
            public bool PhoneNumberConfirmed { get; set; }

            [Display(Name = "Registration Date")]
            public DateTime RegistrationDate { get; set; }

            [Display(Name = "Last Login")]
            public DateTime? LastLogin { get; set; }

            [Display(Name = "Level")]
            [Range(1, int.MaxValue, ErrorMessage = "Level must be at least 1.")]
            public int Level { get; set; } = 1;

            [Display(Name = "Total Points")]
            [Range(0, long.MaxValue, ErrorMessage = "Total points must be a non-negative number.")]
            public long TotalPoints { get; set; } = 0;

            [Display(Name = "Two-Factor Authentication")]
            public bool TwoFactorEnabled { get; set; }
        }

        public class ManageSettings
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current Password")]
            public string CurrentPassword { get; set; } = string.Empty;

            [Required]
            [StringLength(100, MinimumLength = 8,
                ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
            [DataType(DataType.Password)]
            [Display(Name = "New Password")]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
                ErrorMessage = "Password must contain uppercase, lowercase, number, and special character.")]
            public string NewPassword { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Display(Name = "Confirm New Password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = string.Empty;

            [Display(Name = "Email Notifications")]
            public bool EmailNotifications { get; set; } = true;

            [Display(Name = "Push Notifications")]
            public bool PushNotifications { get; set; } = true;

            public bool IsTwoFactorEnabled { get; set; } = false;

        }

        public class DeleteAccountSettings
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            public bool ConfirmDelete { get; set; } = false;

            [Required]
            public string DeleteConfirmation { get; set; } = string.Empty;
        }

        public class ExternalLogin
        {
            public string Provider { get; set; } = string.Empty;
            public string DisplayName { get; set; } = string.Empty;
            public bool IsConnected { get; set; }
        }
    }
}
