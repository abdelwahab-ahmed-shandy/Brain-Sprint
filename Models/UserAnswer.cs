using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserAnswer : BaseModel
    {
        public int UserExamAttempId { get; set; }

        [ForeignKey("UserExamAttempId")]
        public UserExamAttemp UserExamAttemp { get; set; }

        public int QuestionId { get; set; }

        [ForeignKey("QuestionId")]
        public Question Question { get; set; }

        public int ChoiceId { get; set; }

        [ForeignKey("ChoiceId")]
        public Choice Choice { get; set; }
    }
}