
namespace Models
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }
        public CurrentState? CurrentState { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDateUtc { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedDateUtc { get; set; }
        public string? BlockedBy { get; set; }
        public DateTime BlockedDateUtc { get; set; }
    }
}

