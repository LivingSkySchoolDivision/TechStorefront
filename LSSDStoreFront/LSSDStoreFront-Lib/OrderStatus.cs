using System;
using System.Collections.Generic;
using System.Text;

namespace LSSDStoreFront_Lib
{
    class OrderStatus
    {
        public int OrderID { get; set; }
        public string StatusID { get; set; }
        public DateTime OrderTime { get; set; }
        public string Notes { get; set; }
    }
}
