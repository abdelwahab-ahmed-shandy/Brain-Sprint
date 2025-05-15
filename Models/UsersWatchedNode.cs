
namespace Models
{
    public class UsersWatchedNode : BaseModel
    {
        public bool IsWatched { get; set; }

        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        public int NodeId { get; set; }
        [ForeignKey("NodeId")]
        public Node Node { get; set; }
    }
}
