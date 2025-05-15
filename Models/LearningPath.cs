
namespace Models
{
    public class LearningPath : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? IconUrl { get; set; }

        public List<CourseLearningPath> CourseLearningPaths { get; set; } = new List<CourseLearningPath>();
    }
}

