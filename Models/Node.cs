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

        public List<NodeAttachment>? NodeAttachments { get; set; }
        public List<UsersWatchedNode>? UsersWatchedNodes { get; set; }
    }
}

/*
    Title               => عنوان العقدة (مثل العنوان الخاص بالفيديو أو النص)

    NodeType            => نوع العقدة (مثل فيديو، نص، صورة، إلخ)

    Index               => ترتيب العقدة داخل الوحدة

    IsFree              => إذا كانت العقدة مجانية (نعم/لا)

    CourseModuleId      => معرف الوحدة التدريبية التي تنتمي إليها العقدة
    CourseModule        => الكائن الخاص بالوحدة التدريبية المرتبطة

    VidoeNodeId         => معرف العقدة الخاصة بالفيديو (اختياري)

    VideoNode           => الكائن الخاص بالفيديو المرتبط (اختياري)

    TextNodeID          => معرف العقدة الخاصة بالنص (اختياري)

    TextNode            => الكائن الخاص بالنص المرتبط (اختياري)

    NodeAttachments     => قائمة الملفات المرفقة بالعقدة (اختياري)

    UsersWatchedNodes   => قائمة المستخدمين الذين شاهدوا هذه العقدة (اختياري)
*/
