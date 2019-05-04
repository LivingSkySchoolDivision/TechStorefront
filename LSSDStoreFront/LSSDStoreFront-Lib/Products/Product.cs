using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.Lib
{
    public class Product
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Alert { get; set; }
        public string ThumbnailFileName { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsLimitedByStock { get; set; }
        public bool IsLimitedByDate { get; set; }
        public bool IsSpecialOrderItem { get; set; }
        public DateTime AvailableFromDate { get; set; }
        public DateTime AvailableToDate { get; set; }       

    }
}
