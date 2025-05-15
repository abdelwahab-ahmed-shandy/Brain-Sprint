
namespace DataAccess.Repositories
{
    public class ChoiceRepository : Repository<Choice>, IChoiceRepository
    {
        public ChoiceRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
