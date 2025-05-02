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
        public UserExamAttemp UserExamAttemp { get; set; } = null!; // تغيير من new() إلى null!

        public int QuestionId { get; set; }

        [ForeignKey("QuestionId")]
        public Question Question { get; set; } = null!; // تغيير من new() إلى null!

        public int ChoiceId { get; set; }

        [ForeignKey("ChoiceId")]
        public Choice Choice { get; set; } = null!; // تغيير من new() إلى null!
    }
}

/*
    UserExamAttempId => المعرف الخاص بمحاولة اختبار المستخدم المرتبطة بالإجابة

    UserExamAttemp   => الكائن الذي يمثل محاولة اختبار المستخدم المرتبطة بالإجابة

    QuestionId       => المعرف الخاص بالسؤال الذي تم الإجابة عليه

    Question         => الكائن الذي يمثل السؤال المرتبط بالإجابة

    ChoiceId         => المعرف الخاص بالخيار الذي تم اختياره في الإجابة

    Choice           => الكائن الذي يمثل الخيار المختار للإجابة
*/

