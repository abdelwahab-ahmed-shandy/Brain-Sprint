using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Choice : BaseModel
    {
        public string OptionText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }

        [ForeignKey("QuestionId")]
        public Question Question { get; set; } = null!;

        public List<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
    }
}

/*
    OptionText   => نص الخيار في السؤال
    IsCorrect    => يحدد ما إذا كان الخيار صحيحًا (True) أم خاطئًا (False)
    QuestionId   => المعرف الخارجي للسؤال المرتبط بهذا الخيار
    Question     => الكائن الذي يمثل السؤال المرتبط بهذا الخيار
    userAnswers  => قائمة بالإجابات التي اختارها المستخدمين لهذا الخيار
*/