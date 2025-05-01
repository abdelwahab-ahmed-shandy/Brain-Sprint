using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserExamAttemp : BaseModel
    {
        public int ExamScore { get; set; }
        public int UserScore { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        public List<UserAnswer> UserAnswers { get; set; }
    }
}

/*
    ExamScore    => النتيجة التي حصل عليها الطالب في الاختبار

    UserScore    => النتيجة التي سجلها المستخدم بناءً على إجابات الأسئلة

    StartedAt    => تاريخ ووقت بدء الاختبار

    FinishedAt   => تاريخ ووقت إتمام الاختبار

    StudentId    => المعرف الخاص بالطالب الذي أجرى الاختبار

    Student      => الكائن الذي يمثل الطالب الذي أجرى الاختبار

    ExamId       => المعرف الخاص بالاختبار الذي تم التقدم له

    Exam         => الكائن الذي يمثل الاختبار

    UserAnswers  => قائمة بالإجابات التي قدمها المستخدم للأسئلة
*/
