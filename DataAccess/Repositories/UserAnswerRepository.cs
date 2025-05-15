
namespace DataAccess.Repositories
{
    public class UserAnswerRepository : Repository<UserAnswer>, IUserAnswerRepository
    {
        public UserAnswerRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
