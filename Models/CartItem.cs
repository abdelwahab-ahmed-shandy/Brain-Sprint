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
        public Cart Cart { get; set; }

        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }
    }
}

