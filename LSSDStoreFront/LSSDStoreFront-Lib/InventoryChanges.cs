using System;
using System.Collections.Generic;
using System.Text;

namespace LSSDStoreFront_Lib
{
    class InventoryChanges
    {
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public DateTime EventDate { get; set; }
    }
}
