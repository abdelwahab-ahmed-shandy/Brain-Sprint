
namespace DataAccess.Repositories
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        public AdminRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
