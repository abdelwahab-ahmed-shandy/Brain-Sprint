using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Models
{
    public class CourseModule : BaseModel
    {
        public string Title { get; set; } = string.Empty;
        public int Index { get; set; }

        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
        public ICollection<Exam> Exams { get; set; } = new List<Exam>();
        public List<Node> Nodes { get; set; } = new List<Node>();

    }
}


/*
    Title       => عنوان الوحدة الدراسية

    Index       => ترتيب الوحدة في الدورة التدريبية

    CourseId    => معرف الدورة التدريبية التي تنتمي إليها الوحدة

    Course      => الكائن الخاص بالدورة التدريبية المرتبطة

    ExamId      => معرف الاختبار الذي قد يكون مرتبطًا بهذه الوحدة (اختياري)

    Exam        => الكائن الخاص بالاختبار المرتبط (اختياري)

    Nodes       => قائمة العقد (مثل الفيديو، النصوص، الصور، الخ) المرتبطة بهذه الوحدة
*/
