
namespace DataAccess.Repositories
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        public TicketRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
