
namespace DataAccess.Repositories
{
    public class ActivityLogRepository : Repository<ActivityLog>, IActivityLogRepository
    {
        public ActivityLogRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }

}
