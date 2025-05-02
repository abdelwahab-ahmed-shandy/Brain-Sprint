using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Question : BaseModel
    {
        public string QuestionText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int Score { get; set; }

        public int ExamId { get; set; }

        [ForeignKey("ExamId")]
        public Exam Exam { get; set; } = null!;

        public List<Choice> Choices { get; set; } = new List<Choice>();
        public List<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
    }

}

/*
    QuestionText  => نص السؤال الذي يُطرح في الاختبار

    IsCorrect     => حالة السؤال (صحيح أم خاطئ)

    Score         => عدد النقاط التي يمنحها السؤال في حالة الإجابة الصحيحة

    ExamId        => معرف الاختبار الذي ينتمي إليه السؤال

    Exam          => الكائن الخاص بالاختبار الذي يحتوي على هذا السؤال

    Choices       => قائمة الخيارات المتاحة للإجابة على السؤال

    UserAnswers   => قائمة الإجابات التي قدمها المستخدمون لهذا السؤال
*/

