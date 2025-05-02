using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Certificate : BaseModel
    {
        public string StudentScore { get; set; }
        public int EnrollmantCourseId { get; set; }
        [ForeignKey("EnrollmantCourseId")]
        public EnrollmentCourse EnrollmentCourse { get; set; }
    }
}

/*
    StudentScore        => درجة الطالب في الدورة
    EnrollmantCourseId  => المعرف الخارجي للدورة المسجلة المرتبطة بالشهادة
    EnrollmentCourse    => الكائن الذي يمثل الدورة المسجلة المرتبطة بالشهادة
*/
