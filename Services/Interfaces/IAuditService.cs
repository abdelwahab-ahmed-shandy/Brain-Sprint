using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAuditService
    {
        Task LogActivityAsync(string userId, string action, string description, string entityId);
    }
}
