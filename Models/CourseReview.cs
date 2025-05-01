using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CourseReview : BaseModel
    {
        public int Rating { get; set; }
        public string? Comment { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}

/*
    Rating               => تقييم الدورة من قبل الطالب (مثال: من 1 إلى 5)
    Comment              => تعليق الطالب على الدورة (اختياري)

    StudentId            => المعرف الخاص بالطالب الذي قام بتقييم الدورة
    Student              => الكائن الذي يمثل الطالب الذي قام بتقييم الدورة

    CourseId             => المعرف الخاص بالدورة التي تم تقييمها
    Course               => الكائن الذي يمثل الدورة التي تم تقييمها
*/
