using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CartItem : BaseModel
    {
        public double PriceAtPurchase { get; set; }
        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public Cart Cart { get; set; } = new Cart();
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; } = new Course();
    }
}

/*
    PriceAtPurchase  => السعر الذي تم شراء العنصر به في السلة
    CartId           => المعرف الخارجي للسلة المرتبطة بهذا العنصر
    Cart             => الكائن الذي يمثل السلة المرتبطة بهذا العنصر
    CourseId         => المعرف الخارجي للدورة المرتبطة بهذا العنصر
    Course           => الكائن الذي يمثل الدورة المرتبطة بهذا العنصر
*/