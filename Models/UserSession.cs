
namespace Models
{
    public class UserSession
    {
        [Key]
        public string? SessionId { get; set; }

        public string? UserId { get; set; }

        public DateTime LoginTime { get; set; }

        public DateTime? LogoutTime { get; set; }

        public string? IpAddress { get; set; }

        public string? DeviceInfo { get; set; }

        public bool IsActive { get; set; }

        [NotMapped]
        public bool IsCurrent { get; set; }
    }
}