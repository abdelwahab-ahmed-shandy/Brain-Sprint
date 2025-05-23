
namespace Models
{
    public class LearningPath : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? IconUrl { get; set; }

        public bool? IsPublished { get; set; } = false;
        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; }

        public List<CourseLearningPath> CourseLearningPaths { get; set; } = new List<CourseLearningPath>();
    }
}

