using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Cart : BaseModel
    {
        public CartStatusType CartStatus { get; set; }
        public int StudentId { get; set; }
        public Student? Student { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}

/*
    CartStatus   => حالة السلة (نشطة، قيد الدفع، مكتملة، ملغاة، منتهية)
    StudentId    => معرف الطالب المرتبط بالسلة
    Student      => الكائن الذي يمثل الطالب صاحب السلة
    CartItems    => العناصر الموجودة داخل السلة
*/