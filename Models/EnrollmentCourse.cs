
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

        public double? Progress { get; set; }
    }
}
