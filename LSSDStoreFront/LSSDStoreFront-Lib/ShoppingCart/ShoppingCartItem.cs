using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.Lib
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public bool IsNoLongerAvailable { get; set; }        
    }
}
