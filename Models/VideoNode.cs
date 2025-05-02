using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

/*
    VideoUrl     => عنوان الرابط للفيديو المتعلق بالعنصر

    Duration     => مدة الفيديو (اختياري)

    NodeId       => المعرف الخاص بالعنصر الذي يحتوي على الفيديو

    Node         => الكائن الذي يمثل العنصر الذي يحتوي على الفيديو
*/
