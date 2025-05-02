using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UsersWatchedNode : BaseModel
    {
        public bool IsWatched { get; set; }

        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; } = new Student();

        public int NodeId { get; set; }
        [ForeignKey("NodeId")]
        public Node Node { get; set; } = new Node();
    }
}

/*
    IsWatched    => حالة ما إذا كان الطالب قد شاهد هذا العنصر (نعم أو لا)

    StudentId    => المعرف الخاص بالطالب الذي شاهد العنصر

    Student      => الكائن الذي يمثل الطالب الذي شاهد العنصر

    NodeId       => المعرف الخاص بالعنصر الذي شاهده الطالب

    Node         => الكائن الذي يمثل العنصر الذي تم مشاهدته
*/
