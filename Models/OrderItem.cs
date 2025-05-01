using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class OrderItem : BaseModel
    {
        public int Price { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}

/*
    Price        => سعر العنصر الذي تم طلبه في الطلب

    OrderId      => معرف الطلب الذي يتبع له هذا العنصر

    Order        => الكائن الخاص بالطلب الذي يحتوي على هذا العنصر

    CourseId     => معرف الدورة التعليمية التي تم طلبها

    Course       => الكائن الخاص بالدورة التعليمية التي تم طلبها
*/

