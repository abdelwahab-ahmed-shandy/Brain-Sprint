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
        public Node Node { get; set; } = new Node();
    }
}

/*
    Title               => عنوان الملف المرفق

    Description         => وصف للملف المرفق (اختياري)

    FileUrl             => رابط الملف المرفق

    NodeId              => معرف العقدة التي يرتبط بها هذا المرفق

    Node                => الكائن الخاص بالعقدة المرتبطة بالملف المرفق
*/

