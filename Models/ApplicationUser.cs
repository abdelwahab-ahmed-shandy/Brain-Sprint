﻿using Microsoft.AspNetCore.Identity;
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
        // Add this new property
        public DateTime? PasswordChangedDate { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

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


        public string? Certifications { get; set; }
        public int? ExperienceYears { get; set; }

        public AccountStateType? AccountState { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }


        // One-to-One Relationship Between ApplicationUser and Student/Instructor/Admin
        public Student? Student { get; set; }
        public Instructor? Instructor { get; set; }
        public Admin? Admin { get; set; }

        // One-to-Many from ApplicationUser to Tickets
        public IEnumerable<Ticket>? Tickets { get; set; } = new List<Ticket>();

    }
}
