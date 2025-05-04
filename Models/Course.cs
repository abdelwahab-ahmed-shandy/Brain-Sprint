using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Course : BaseModel
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public double Price { get; set; }
        public double? Discount { get; set; }
        public int Duration { get; set; }
        public string? VideoUrl { get; set; }
        public string? ImgUrl { get; set; }

        [Required]
        public int InstructorId { get; set; }
        [ForeignKey("InstructorId")]
        public Instructor? Instructor { get; set; }

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public List<CourseLearningPath> CourseLearningPaths { get; set; } = new List<CourseLearningPath>();
        public List<CourseReview> CourseReviews { get; set; } = new List<CourseReview>();
        public List<EnrollmentCourse> EnrollmentCourses { get; set; } = new List<EnrollmentCourse>();
        public List<CourseModule> CourseModules { get; set; } = new List<CourseModule>();
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
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