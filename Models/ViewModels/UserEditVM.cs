using Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class UserEditVM
    {
        public string? Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }



        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }



        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Account State")]
        public AccountStateType AccountState { get; set; }

        public string? Certifications { get; set; }
        public int? ExperienceYears { get; set; }
    }
}
