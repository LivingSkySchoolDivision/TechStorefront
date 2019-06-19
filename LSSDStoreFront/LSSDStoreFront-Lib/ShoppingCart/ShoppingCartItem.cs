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

        public decimal TotalPriceWithTax {
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

        public decimal TotalBasePrice
        {
            get
            {
                if (Product != null)
                {
                    return Product.BasePrice * Quantity;
                }
                else
                {
                    return 0;
                }
            }
        }
        public decimal TotalGST
        {
            get
            {
                if (Product != null)
                {
                    return Product.GSTAmount * Quantity;
                }
                else
                {
                    return 0;
                }
            }
        }


        public decimal TotalPST
        {
            get
            {
                if (Product != null)
                {
                    return Product.PSTAmount * Quantity;
                }
                else
                {
                    return 0;
                }
            }
        }

        public decimal TotalEHF
        {
            get
            {
                if (Product != null)
                {
                    return Product.RecyclingFee * Quantity;
                }
                else
                {
                    return 0;
                }
            }
        }




    }
}
