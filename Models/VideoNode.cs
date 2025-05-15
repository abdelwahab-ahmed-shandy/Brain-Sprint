
namespace Models
{
    public class VideoNode : BaseModel
    {
        public string VideoUrl { get; set; } = string.Empty;
        public TimeSpan? Duration { get; set; }

        public int NodeId { get; set; }

        [ForeignKey("NodeId")]
        public Node Node { get; set; }
    }
}