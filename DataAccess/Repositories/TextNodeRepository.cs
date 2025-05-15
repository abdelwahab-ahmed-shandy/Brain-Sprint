
namespace DataAccess.Repositories
{
    public class TextNodeRepository : Repository<TextNode>, ITextNodeRepository
    {
        public TextNodeRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
