using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Course : BaseModel
    {
        public string Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public TimeSpan? Duration { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? ImgUrl { get; set; }

        public int? InstructorId { get; set; }
        public Instructor? Instructor { get; set; }

        public List<CartItem> CartItems { get; set; }
        public List<CourseLearningPath> CourseLearningPaths { get; set; }
        public List<CourseReview> CourseReviews { get; set; }
        public List<EnrollmentCourse> EnrollmentCourses { get; set; }
        public List<CourseModule> CourseModules { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}

/*
    Title                 => عنوان الدورة
    ShortDescription      => وصف مختصر للدورة
    LongDescription       => وصف طويل للدورة
    Price                 => سعر الدورة
    Discount              => نسبة الخصم (اختياري)
    Duration              => مدة الدورة (اختياري)
    ThumbnailUrl          => رابط الصورة المصغرة للدورة
    ImgUrl                => رابط الصورة الرئيسية للدورة

    InstructorId          => المعرف الخارجي للمدرب
    Instructor            => الكائن الذي يمثل المدرب المسؤول عن الدورة

    CartItems             => قائمة العناصر الموجودة في السلة المرتبطة بهذه الدورة
    CourseLearningPaths   => قائمة مسارات التعلم المرتبطة بالدورة
    CourseReviews         => قائمة المراجعات الخاصة بالدورة
    EnrollmentCourses     => قائمة الدورات المسجلة من قبل الطلاب
    Modules               => قائمة الوحدات الدراسية التي تتضمنها الدورة
    OrderItems            => قائمة العناصر في الطلبات المرتبطة بالدورة
*/