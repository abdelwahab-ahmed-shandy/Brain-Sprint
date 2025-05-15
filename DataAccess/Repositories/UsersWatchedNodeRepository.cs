
namespace DataAccess.Repositories
{
    public class UsersWatchedNodeRepository : Repository<UsersWatchedNode>, IUsersWatchedNodeRepository
    {
        public UsersWatchedNodeRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
