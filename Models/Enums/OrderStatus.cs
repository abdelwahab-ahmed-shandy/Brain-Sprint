using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums
{
    public enum OrderStatus
    {
        Pending = 1,
        Canceled = 2,
        InProgress = 3,
        Shipped = 4,
        Completed = 5
    }
}
