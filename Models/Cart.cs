
namespace Models
{
    public class Cart : BaseModel
    {
        public CartStatusType? CartStatus { get; set; }

        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public int Count { get; set; }

    }
}

