
namespace Models
{
    public class EnrollmentCourse : BaseModel
    {
        public DateTime EnrollmentDate { get; set; }

        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        public Certificate? Certificate { get; set; }

        [Range(0, 100, ErrorMessage = "Progress must be between 0 and 100")]
        public int? Progress { get; set; } = 0;

        public bool IsCompleted => Progress.HasValue && Progress.Value == 100;

    }
}
