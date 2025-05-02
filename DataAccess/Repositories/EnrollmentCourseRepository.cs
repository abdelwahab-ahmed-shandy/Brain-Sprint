using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class EnrollmentCourseRepository : Repository<EnrollmentCourse>, IEnrollmentCourseRepository
    {
        public EnrollmentCourseRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
