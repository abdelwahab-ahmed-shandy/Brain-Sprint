
namespace DataAccess.Repositories
{
    public class CourseReviewRepository : Repository<CourseReview>, ICourseReviewRepository
    {
        public CourseReviewRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
