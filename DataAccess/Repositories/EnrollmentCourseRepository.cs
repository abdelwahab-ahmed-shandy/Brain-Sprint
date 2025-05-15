
namespace DataAccess.Repositories
{
    public class EnrollmentCourseRepository : Repository<EnrollmentCourse>, IEnrollmentCourseRepository
    {
        public EnrollmentCourseRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
