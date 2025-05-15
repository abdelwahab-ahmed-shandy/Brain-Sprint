
namespace Models
{
    public class CourseModule : BaseModel
    {
        public string Title { get; set; } = string.Empty;
        public int Index { get; set; }

        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course? Course { get; set; }

        public ICollection<Exam> Exams { get; set; } = new List<Exam>();

        public ICollection<Node> Nodes { get; set; } = new List<Node>();
    }
}
