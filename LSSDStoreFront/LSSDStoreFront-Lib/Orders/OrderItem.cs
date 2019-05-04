using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.Lib
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerItem { get; set; }
        public decimal TotalPrice { get
            {
                return this.PricePerItem * this.Quantity;
            }
        }
    }
}
