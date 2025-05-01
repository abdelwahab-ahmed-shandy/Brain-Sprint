using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Exam : BaseModel
    {
        public string Title { get; set; }
        public int Score { get; set; }
        public TimeSpan? MaximumTime { get; set; }

        public int CourseModuleId { get; set; }
        public CourseModule CourseModule { get; set; }

        public List<Question> Questions { get; set; }
        public List<UserExamAttemp> UserExamAttemps { get; set; }
    }
}

/*
    Title                => عنوان الامتحان

    Score                => الدرجة الإجمالية للامتحان

    MaximumTime          => الحد الأقصى للوقت المسموح به للإجابة على الامتحان (اختياري)

    CourseModuleId       => المعرف الخاص بالوحدة التي يتبعها الامتحان
    CourseModule               => الكائن الذي يمثل الوحدة التي يحتوي عليها الامتحان

    Questions            => قائمة الأسئلة التي يتضمنها الامتحان

    UserExamAttemps      => قائمة المحاولات التي قام بها الطلاب للامتحان
*/
