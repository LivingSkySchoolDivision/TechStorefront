using System;
using System.Collections.Generic;
using System.Text;

namespace LSSDStoreFront_Lib
{
    class ProductCategory
    {        
        public int ID { get; set; }
        public string Name { get; set; }
        public string ParentCategory { get; set; }        
        public DateTime AvailableFromDate { get; set; }
        public DateTime AvailableToDate { get; set; }
    }
}
