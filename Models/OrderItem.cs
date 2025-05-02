using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class OrderItem : BaseModel
    {
        public int Price { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; } = new Order();

        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; } = new Course();
    }
}

/*
    Price        => سعر العنصر الذي تم طلبه في الطلب

    OrderId      => معرف الطلب الذي يتبع له هذا العنصر

    Order        => الكائن الخاص بالطلب الذي يحتوي على هذا العنصر

    CourseId     => معرف الدورة التعليمية التي تم طلبها

    Course       => الكائن الخاص بالدورة التعليمية التي تم طلبها
*/

