using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.Lib
{
    public enum OrderStatus
    {
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
