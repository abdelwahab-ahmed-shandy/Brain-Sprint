
namespace DataAccess.Repositories
{
    public class LearningPathRepository : Repository<LearningPath>, ILearningPathRepository
    {
        public LearningPathRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
