using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.Lib
{
    public class OrderItem
    {
        public string OrderThumbprint { get; set; }
        public string Name { get; set; }
        public decimal ItemBasePrice { get; set; }
        public decimal ItemGST { get; set; }
        public decimal ItemPST { get; set; }
        public decimal ItemEHF { get; set; }
        public decimal ItemPriceWithTax { get; set; }
        public decimal TotalBasePrice { get; set; }
        public decimal TotalGST { get; set; }
        public decimal TotalPST { get; set; }
        public decimal TotalEHF { get; set; }
        public decimal TotalPriceWithTax { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }       
        
    }
}
