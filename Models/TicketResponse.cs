using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TicketResponse : BaseModel
    {
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }

        public string ResponderUserId { get; set; }
        public ApplicationUser ResponderUser { get; set; }

        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
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

