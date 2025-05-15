
namespace Models.ViewModels
{
    public class UserVM
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

        public bool IsBlocked { get; set; }

        public string? Certifications { get; set; }
        public int? ExperienceYears { get; set; }
    }



    public class SuperAdminListVM
    {
        public IEnumerable<UserVM> SuperAdmins { get; set; }
        public PaginationVM Pagination { get; set; }
    }

    public class AdminListVM
    {
        public IEnumerable<UserVM> Admins { get; set; }
        public PaginationVM Pagination { get; set; }
    }

    public class InstructorListVM
    {
        public IEnumerable<UserVM> Instructors { get; set; }
        public PaginationVM Pagination { get; set; }
    }

    public class StudentListVM
    {
        public IEnumerable<UserVM> Students { get; set; }
        public PaginationVM Pagination { get; set; }
    }

}
