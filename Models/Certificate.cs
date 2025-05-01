using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Certificate : BaseModel
    {
        public string StudentScore { get; set; }
        public int EnrollmantCourseId { get; set; }
        public EnrollmentCourse EnrollmentCourse { get; set; }
    }
}

/*
    StudentScore        => درجة الطالب في الدورة
    EnrollmantCourseId  => المعرف الخارجي للدورة المسجلة المرتبطة بالشهادة
    EnrollmentCourse    => الكائن الذي يمثل الدورة المسجلة المرتبطة بالشهادة
*/
