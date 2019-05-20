using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.Lib
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int ProductId { get; set; }
        public bool Fulfilled { get; set; }
        public DateTime FulfilledDate { get; set; }
    }
}
