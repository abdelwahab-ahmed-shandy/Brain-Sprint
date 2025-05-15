
namespace DataAccess.Repositories
{
    public class CourseLearningPathRepository : Repository<CourseLearningPath>, ICourseLearningPathRepository
    {
        public CourseLearningPathRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
