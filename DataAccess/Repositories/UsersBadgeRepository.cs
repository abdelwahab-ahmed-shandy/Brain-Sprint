
namespace DataAccess.Repositories
{
    public class UsersBadgeRepository : Repository<UsersBadge>, IUsersBadgeRepository
    {
        public UsersBadgeRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
