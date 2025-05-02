using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class CourseLearningPathRepository : Repository<CourseLearningPath>, ICourseLearningPathRepository
    {
        public CourseLearningPathRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
