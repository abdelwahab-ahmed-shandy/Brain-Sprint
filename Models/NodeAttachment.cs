using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class NodeAttachment : BaseModel
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string FileUrl { get; set; }

        public int NodeId { get; set; }
        public Node Node { get; set; }
    }
}

/*
    Title               => عنوان الملف المرفق

    Description         => وصف للملف المرفق (اختياري)

    FileUrl             => رابط الملف المرفق

    NodeId              => معرف العقدة التي يرتبط بها هذا المرفق

    Node                => الكائن الخاص بالعقدة المرتبطة بالملف المرفق
*/

