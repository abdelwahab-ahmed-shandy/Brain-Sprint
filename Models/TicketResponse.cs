using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TicketResponse : BaseModel
    {
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public int AdminId { get; set; }
        [ForeignKey("AdminId")]
        public Admin Admin { get; set; } = null!;


        public int TicketId { get; set; }
        [ForeignKey("TicketId")]
        public Ticket Ticket { get; set; } = new Ticket();
    }
}

/*
    Message         => الرسالة التي يرد بها المستخدم على التذكرة

    CreatedAt       => تاريخ ووقت إنشاء الرد

    ResponderUserId => المعرف الخاص بالمستخدم الذي قام بالرد

    ResponderUser   => الكائن الذي يمثل المستخدم الذي قام بالرد

    TicketId        => المعرف الخاص بالتذكرة المرتبطة بالرد

    Ticket          => الكائن الذي يمثل التذكرة المرتبطة بالرد
*/

