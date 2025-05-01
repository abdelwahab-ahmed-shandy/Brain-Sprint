using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Instructor : BaseModel
    {
        public string? Certifications { get; set; }
        public string? ExperienceYears { get; set; }
        public int? Rating { get; set; }
        public bool IsVerified { get; set; }

        public string? ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public List<Course> Courses { get; set; } = new List<Course>();
    }
}

/*

    Certifications        => الشهادات التي حصل عليها المدرب (اختياري)

    ExperienceYears       => عدد سنوات الخبرة التي يمتلكها المدرب (اختياري)

    Rating               => تقييم المدرب من قبل الطلاب (اختياري)

    IsVerified           => حالة التحقق من المدرب (يتم التحقق من صحة البيانات في حال كان المدرب موثوقًا)

    ApplicationUserId    => المعرف الخاص بالمستخدم (المدرب) في تطبيق المستخدمين

    ApplicationUser      => الكائن الذي يمثل تطبيق المستخدم (المدرب) في النظام

    Courses              => قائمة الدورات التي يقدمها المدرب
*/
