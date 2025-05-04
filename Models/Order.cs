using Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Order : BaseModel
    {
        public double TotalAmount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}

