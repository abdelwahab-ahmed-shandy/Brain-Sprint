
namespace DataAccess.Repositories
{
    public class BadgeRepository : Repository<Badge>, IBadgeRepository
    {
        public BadgeRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }

}
