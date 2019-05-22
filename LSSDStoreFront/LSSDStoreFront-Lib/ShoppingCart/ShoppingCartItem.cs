using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.Lib
{
    public class ShoppingCartItem
    {
        public string UserThumbprint { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }

        public decimal TotalPrice {
            get
            {
                if (Product != null)
                {
                    return Product.TotalPrice * Quantity;
                } else
                {
                    return 0;
                }
            }
        }
    }
}
