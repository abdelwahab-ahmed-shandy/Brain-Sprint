using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Order : BaseModel
    {
        public decimal TotalAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}

/*
    TotalAmount          => إجمالي المبلغ المطلوب دفعه للطلب

    PaymentMethod        => طريقة الدفع المستخدمة في الطلب

    StudentId            => معرف الطالب الذي قام بالطلب

    Student              => الكائن الخاص بالطالب الذي قام بالطلب

    OrderItems           => قائمة بالعناصر (المنتجات) التي تم طلبها في هذا الطلب
*/

