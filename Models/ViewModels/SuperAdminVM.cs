using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class SuperAdminVM
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? ProfileImage { get; set; }
        public string? PhoneNumber { get; set; }
        public AccountStateType AccountState { get; set; }
        public string Status { get; set; }
        public string? Bio { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime? LastLogin { get; set; }
    }

    public class PaginationVM
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public string Query { get; set; }
        public string StatusFilter { get; set; }
    }

    public class SuperAdminListVM
    {
        public IEnumerable<SuperAdminVM> SuperAdmins { get; set; }
        public PaginationVM Pagination { get; set; }
    }
}
