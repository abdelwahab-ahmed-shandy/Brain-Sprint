
namespace Models
{
    public class Certificate : BaseModel
    {
        public string StudentScore { get; set; }

        public int EnrollmentCourseId { get; set; }

        [ForeignKey("EnrollmentCourseId")]
        public EnrollmentCourse EnrollmentCourse { get; set; }
    }
}
