using Microsoft.AspNetCore.Identity;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
        public string? FirstName { get; set; }

        [MaxLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
        public string? LastName { get; set; }

        [MaxLength(500, ErrorMessage = "Address cannot exceed 500 characters.")]
        public string? Address { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }
        public DateTime? LastLogin { get; set; }

        [RegularExpression(@"^.*\.(jpg|jpeg|png)$")]
        public string? ProfileImage { get; set; }

        [Range(0, long.MaxValue, ErrorMessage = "Total points must be a non-negative number.")]
        public long TotalPoints { get; set; } = 0;

        [Range(1, int.MaxValue, ErrorMessage = "Level must be at least 1.")]
        public int Level { get; set; } = 1;

        [MaxLength(1000, ErrorMessage = "Bio cannot exceed 1000 characters.")]
        public string? Bio { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsBlocked { get; set; } = false;
        public DateTime? BlockedDate { get; set; }

        [MaxLength(1000, ErrorMessage = "Block reason cannot exceed 1000 characters.")]
        public string? BlockReason { get; set; }


        public AccountStateType AccountState { get; set; } = AccountStateType.PendingActivation;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }


        public int? StudentId { get; set; }
        public Student? Student { get; set; }

        public int? InstructorId { get; set; }
        public Instructor? Instructor { get; set; }

        public int? AdminId { get; set; }
        public Admin? Admin { get; set; }

        public IEnumerable<Ticket>? Tickets { get; set; }


    }
}
