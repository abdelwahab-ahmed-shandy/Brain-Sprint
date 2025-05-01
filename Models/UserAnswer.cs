using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserAnswer : BaseModel
    {
        public int UserExamAttempId { get; set; }
        public UserExamAttemp UserExamAttemp { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public int ChoiceId { get; set; }
        public Choice Choice { get; set; }
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

