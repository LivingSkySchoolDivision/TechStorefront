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


        public bool IsTopLevel
        {
            get
            {
                return this.ParentCategoryId == 0;
            }
        }

        public bool HasParent {
            get
            {
                return this.ParentCategoryId > 0;
            }
        }

        public bool HasChildren {
            get
            {
                return this.ChildCategories.Count > 0;
            }
        }

        public ProductCategory ParentCategory { get; set; }
        public List<ProductCategory> ChildCategories { get; set; }

        public ProductCategory()
        {
            this.ParentCategory = null;
            this.ChildCategories = new List<ProductCategory>();
        }
    }
}
