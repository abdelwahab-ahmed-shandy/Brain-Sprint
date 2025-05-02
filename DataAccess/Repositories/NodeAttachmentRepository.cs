using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class NodeAttachmentRepository : Repository<NodeAttachment>, INodeAttachmentRepository
    {
        public NodeAttachmentRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
