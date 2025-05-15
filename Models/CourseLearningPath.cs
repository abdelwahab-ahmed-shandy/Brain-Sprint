
namespace Models
{
    // Use the Unique Index in CourseLearningPath to prevent the same course from being linked to the same path more than once:
    [Index(nameof(CourseId), nameof(LearningPathId), IsUnique = true)]
    public class CourseLearningPath : BaseModel
    {
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; } = new Course();

        public int LearningPathId { get; set; }
        [ForeignKey("LearningPathId")]
        public LearningPath LearningPath { get; set; } = new LearningPath();
    }
}


