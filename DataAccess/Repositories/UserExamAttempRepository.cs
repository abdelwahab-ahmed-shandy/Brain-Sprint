
namespace DataAccess.Repositories
{
    public class UserExamAttempRepository : Repository<UserExamAttemp>, IUserExamAttempRepository
    {
        public UserExamAttempRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
