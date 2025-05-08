using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums
{
    public enum CartStatusType
    {
        InCart = 1,
        PendingPayment = 2,
        Completed = 3,
        Cancelled = 4,
        Expired = 5
    }
}

