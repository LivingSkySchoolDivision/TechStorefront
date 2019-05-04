using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.Lib
{
    public class ProductCategory
    {        
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentCategoryId { get; set; } 
        public bool IsEnabled { get; set; }
        public DateTime AvailableFromDate { get; set; }
        public DateTime AvailableToDate { get; set; }
    }
}
