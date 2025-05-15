
namespace Models
{
    public class TextNode : BaseModel
    {
        public string Text { get; set; } = string.Empty;
        public long? Length { get; set; }

        public int NodeId { get; set; }
        [ForeignKey("NodeId")]
        public Node Node { get; set; }
    }
}