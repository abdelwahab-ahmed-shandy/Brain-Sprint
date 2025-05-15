
namespace DataAccess.Repositories
{
    public class ExamRepository : Repository<Exam>, IExamRepository
    {
        public ExamRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
