using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CartItem : BaseModel
    {
        public decimal PriceAtPurchase { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}

/*
    PriceAtPurchase  => السعر الذي تم شراء العنصر به في السلة
    CartId           => المعرف الخارجي للسلة المرتبطة بهذا العنصر
    Cart             => الكائن الذي يمثل السلة المرتبطة بهذا العنصر
    CourseId         => المعرف الخارجي للدورة المرتبطة بهذا العنصر
    Course           => الكائن الذي يمثل الدورة المرتبطة بهذا العنصر
*/