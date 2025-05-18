using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class AuditService : IAuditService
    {
        private readonly ApplicationDbContext _context;

        public AuditService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogActivityAsync(string userId, string action, string description, string entityId)
        {
            var auditRecord = new AuditRecord
            {
                UserId = userId,
                Action = action,
                Description = description,
                EntityId = entityId,
                Timestamp = DateTime.UtcNow
            };

            await _context.AuditRecords.AddAsync(auditRecord);
            await _context.SaveChangesAsync();
        }
    }

}
