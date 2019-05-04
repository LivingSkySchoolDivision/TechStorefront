using System;
using System.Collections.Generic;
using System.Text;

namespace LSSDStoreFront_Lib
{
    class CartItem
    {
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public bool IsRemovedBySystem { get; set; }
    }
}
