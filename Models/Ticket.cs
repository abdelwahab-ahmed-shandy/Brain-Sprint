using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Ticket : BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketStatusType Status { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? ColsedAt { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }

        public List<TicketResponse> TicketResponses { get; set; }
    }
}

/*
    Title          => عنوان التذكرة

    Description    => وصف التذكرة

    Status         => حالة التذكرة (مثال: مفتوحة، مغلقة)

    CreateAt       => تاريخ ووقت إنشاء التذكرة

    ColsedAt       => تاريخ ووقت غلق التذكرة (اختياري)

    ApplicationUserId => المعرف الخاص بالمستخدم الذي قدم التذكرة

    User           => الكائن الذي يمثل المستخدم المرتبط بالتذكرة

    TicketResponses => قائمة من الردود التي تم تقديمها على التذكرة
*/

