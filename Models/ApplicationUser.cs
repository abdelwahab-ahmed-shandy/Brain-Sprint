
namespace Models
{
    public class ApplicationUser : IdentityUser
    {
        // Add this new property
        public DateTime? PasswordChangedDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? BirthDay { get; set; }
        public string? ProfileImage { get; set; }
        public string? Bio { get; set; }
        public string? BlockReason { get; set; }
        public string? Certifications { get; set; }
        public int Level { get; set; } = 1;
        public int? ExperienceYears { get; set; }
        public long TotalPoints { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public bool IsBlocked { get; set; } = false;

        // Base Model Properties
        public LevelType? LevelType { get; set; } = Enums.LevelType.Beginner;
        public AccountStateType? AccountState { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDateUtc { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedDateUtc { get; set; }
        public string? BlockedBy { get; set; }
        public DateTime BlockedDateUtc { get; set; }

        // One-to-One Relationship Between ApplicationUser and Student/Instructor/Admin
        public Student? Student { get; set; }
        public Instructor? Instructor { get; set; }
        public Admin? Admin { get; set; }

        // One-to-Many from ApplicationUser to Tickets
        public IEnumerable<Ticket>? Tickets { get; set; } = new List<Ticket>();

    }
}
