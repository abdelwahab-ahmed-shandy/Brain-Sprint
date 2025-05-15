
namespace DataAccess.Repositories
{
    public class TicketResponseRepository : Repository<TicketResponse>, ITicketResponseRepository
    {
        public TicketResponseRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
