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
        public Question Question { get; set; }

        public List<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
    }
}

