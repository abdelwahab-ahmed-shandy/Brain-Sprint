using Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Student : BaseModel
    {
        public LevelType? Level { get; set; }

        public string? ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }


        public List<Cart> Carts { get; set; }
        public List<CartItem> CartItems { get; set; }
        public List<CourseReview> CourseReviews { get; set; }
        public List<EnrollmentCourse> EnrollmentCourses { get; set; }
        public List<Order> Orders { get; set; }
        public List<UserExamAttemp> UserExamAttemps { get; set; }
        public List<UsersBadge> UsersBadges { get; set; }
        public List<UsersWatchedNode> UsersWatchedNodes { get; set; }
    }
}

/*
    Level      => المستوى الدراسي للطالب (قد يكون غير مُحدد)
    
    UserId     => المعرف الخاص بالمستخدم (يتم ربطه بكائن ApplicationUser)

    User       => الكائن الخاص بالمستخدم المرتبط بالطالب

    Carts      => قائمة السلال المشتراة من قبل الطالب

    CartItems  => قائمة العناصر المضافة إلى سلة الطالب

    CourseReviews => قائمة تقييمات الدورات التي قدمها الطالب

    EnrollmentCourses => قائمة الدورات التي قام الطالب بالتسجيل فيها

    Orders     => قائمة الطلبات التي أتمها الطالب

    UserExamAttemps => قائمة محاولات الاختبارات التي قام بها الطالب

    UsersBadges => قائمة الشهادات أو الجوائز التي حصل عليها الطالب

    UsersWatchedNodes => قائمة العقد التي قام الطالب بمشاهدتها
*/
