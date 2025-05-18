
namespace Models
{
    public class Instructor : BaseModel
    {
        public int? Rating { get; set; }
        public bool IsVerified { get; set; }

        [ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public List<Course> Courses { get; set; } = new List<Course>();
        public List<LearningPath> LearningPaths { get; set; } = new List<LearningPath>();
    }
}