
namespace Models
{
    public class UserExamAttemp : BaseModel
    {
        public int ExamScore { get; set; }
        public int UserScore { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }

        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; } = new Student();

        public int ExamId { get; set; }
        [ForeignKey("ExamId")]
        public Exam Exam { get; set; }

        public List<UserAnswer> UserAnswers { get; set; }
    }
}