using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Admin : BaseModel
    {
        public IEnumerable<TicketResponse> TicketResponses { get; set; } = new List<TicketResponse>();

        public string? ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}

/*
 
    TicketResponses => قائمة ردود التذاكر التي قدمها المسؤول
    UserID          => المعرف الخاص بالمستخدم (يتم ربطه بكائن ApplicationUser)
    User            => الكائن الخاص بالمستخدم المرتبط بالمسؤول
 
 */
