using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

/*
    Text      => النص الموجود في العقدة النصية

    Length    => طول النص (اختياري، قد يكون فارغًا)

    NodeId    => المعرف الخاص بالعقدة المرتبطة بهذا النص

    Node      => الكائن الذي يمثل العقدة التي تحتوي على النص
*/
