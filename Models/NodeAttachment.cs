using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class NodeAttachment : BaseModel
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string FileUrl { get; set; } = string.Empty;

        public int NodeId { get; set; }
        [ForeignKey("NodeId")]
        public Node Node { get; set; }
    }
}


