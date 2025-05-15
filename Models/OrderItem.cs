
namespace Models
{
    public class OrderItem : BaseModel
    {
        public double Price { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        //new 
        public int Count { get; set; }
    }
}