using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class EnrollmentCourse : BaseModel
    {
        public DateTime EnrollmentDate { get; set; }

        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; } = new Student();

        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; } = new Course();

        public int? CertificateId { get; set; }
        public Certificate? Certificate { get; set; }
    }
}

/*
    EnrollmentDate        => تاريخ تسجيل الطالب في الدورة

    StudentId            => المعرف الخاص بالطالب الذي سجل في الدورة
    Student              => الكائن الذي يمثل الطالب المسجل في الدورة

    CourseId             => المعرف الخاص بالدورة التي تم التسجيل فيها
    Course               => الكائن الذي يمثل الدورة التي تم التسجيل فيها

    CertificateId        => المعرف الخاص بالشهادة التي تم إصدارها (اختياري)
    Certificate          => الكائن الذي يمثل الشهادة المرتبطة بالتسجيل (اختياري)
*/

