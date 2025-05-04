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