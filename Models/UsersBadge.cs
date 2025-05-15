
namespace Models
{
    [Index(nameof(StudentId), nameof(BadgeId), IsUnique = true)]
    public class UsersBadge : BaseModel
    {
        public DateTime AwardedDate { get; set; } = DateTime.UtcNow;

        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; } = null!;

        public int BadgeId { get; set; }
        [ForeignKey("BadgeId")]
        public Badge Badge { get; set; } = null!;
    }
}