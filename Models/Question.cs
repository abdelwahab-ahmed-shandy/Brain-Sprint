
namespace Models
{
    public class Question : BaseModel
    {
        public string QuestionText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int Score { get; set; }

        public int ExamId { get; set; }

        [ForeignKey("ExamId")]
        public Exam Exam { get; set; }

        public List<Choice> Choices { get; set; }
        public List<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
    }

}
