
namespace Models
{
    public class Admin : BaseModel
    {
        public IEnumerable<TicketResponse> TicketResponses { get; set; } = new List<TicketResponse>();

        [ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}


