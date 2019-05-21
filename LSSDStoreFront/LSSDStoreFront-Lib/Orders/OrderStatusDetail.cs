using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.Lib
{
    public class OrderStatusDetail
    {
        public int Id { get; set; }
        public string OrderThumbprint { get; set; }
        public string Status { get; set; }
        public DateTime Timestamp { get; set; }
        public string Notes { get; set; }
        public string UpdatedBy { get; set; }
    }
}
