using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.Lib
{
    public enum OrderStatuses
    {
        Unknown,
        Received,
        Fulfilled,
        Shipped,
        Complete,
        Invalid,
        CancelledByUser,
        CancelledByManagement,
        CancelledByAdmin,
        CancelledNSF,
        CancelledNotAvailable
    }
}
