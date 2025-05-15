
namespace DataAccess.Repositories
{
    public class CourseModuleRepository : Repository<CourseModule>, ICourseModuleRepository
    {
        public CourseModuleRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
