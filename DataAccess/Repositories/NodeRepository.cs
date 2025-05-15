
namespace DataAccess.Repositories
{
    public class NodeRepository : Repository<Node>, INodeRepository
    {
        public NodeRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
