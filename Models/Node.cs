using Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Node : BaseModel
    {
        public string Title { get; set; } = string.Empty;
        public NodeType? NodeType { get; set; }
        public int Index { get; set; }
        public bool IsFree { get; set; }

        public int CourseModuleId { get; set; }
        [ForeignKey("CourseModuleId")]
        public CourseModule CourseModule { get; set; }

        public int? VideoNodeId { get; set; }
        public VideoNode? VideoNode { get; set; }

        public int? TextNodeId { get; set; }
        public TextNode? TextNode { get; set; }

        public ICollection<NodeAttachment>? NodeAttachments { get; set; }

        public ICollection<UsersWatchedNode>? UsersWatchedNodes { get; set; }
    }
}


