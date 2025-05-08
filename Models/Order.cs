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

        public PaymentMethod PaymentMethod { get; set; }

        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        // new 
        public double TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public bool PaymentStatus { get; set; }
        public bool OrderShipedStatus { get; set; }
        public string? Carrier { get; set; }
        public string? TrackingNumber { get; set; }

        public string? SessionId { get; set; }
        public string? PaymentStripeId { get; set; }


    }
}

