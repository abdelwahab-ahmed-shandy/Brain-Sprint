using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class CourseReviewRepository : Repository<CourseReview>, ICourseReviewRepository
    {
        public CourseReviewRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
