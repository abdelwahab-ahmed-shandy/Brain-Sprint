
namespace Models
{
    public class Order : BaseModel
    {
        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; }


        public string? Carrier { get; set; }
        public string? SessionId { get; set; }
        public double TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public bool PaymentStatus { get; set; }
        public bool OrderShipedStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? PaymentStripeId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}

