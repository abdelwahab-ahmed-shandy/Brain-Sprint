﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        Completed,
        Shipped,
        Delivered,
        Canceled,
        Refunded
    }
}
