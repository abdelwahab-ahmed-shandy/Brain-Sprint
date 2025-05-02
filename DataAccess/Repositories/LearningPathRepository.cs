using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class LearningPathRepository : Repository<LearningPath>, ILearningPathRepository
    {
        public LearningPathRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
