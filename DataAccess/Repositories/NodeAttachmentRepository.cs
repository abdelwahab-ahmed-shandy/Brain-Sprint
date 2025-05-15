
namespace DataAccess.Repositories
{
    public class NodeAttachmentRepository : Repository<NodeAttachment>, INodeAttachmentRepository
    {
        public NodeAttachmentRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
