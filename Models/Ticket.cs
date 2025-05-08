using Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Ticket : BaseModel
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketStatusType? Status { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? ColsedAt { get; set; }


        public string CreatedByUserId { get; set; }
        [ForeignKey("CreatedByUserId")]
        public ApplicationUser CreatedByUser { get; set; } = null!;
        public List<TicketResponse> TicketResponses { get; set; } = new List<TicketResponse>();

        //new 
        public TicketPriorityType Priority { get; set; }


    }
}

