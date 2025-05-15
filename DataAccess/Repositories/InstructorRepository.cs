
namespace DataAccess.Repositories
{
    public class InstructorRepository : Repository<Instructor>, IInstructorRepository
    {
        public InstructorRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
