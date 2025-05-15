
namespace Models
{
    public class ActivityLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; } = null!;

        [Required]
        public string Action { get; set; } = string.Empty;

        [StringLength(500)]
        public string Details { get; set; } = string.Empty;

        public ActivityStatus Status { get; set; } = ActivityStatus.Success;

    }
}
