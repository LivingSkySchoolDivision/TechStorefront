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
        public string LongDescription { get; set; }
        public string Alert { get; set; }
        public string ThumbnailFileName { get; set; }
        public decimal RecyclingFee { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsLimitedByStock { get; set; }
        public bool IsLimitedByDate { get; set; }
        public bool IsSpecialOrderItem { get; set; }
        public bool IsGSTExempt { get; set; }
        public bool IsPSTExempt { get; set; }
        public DateTime AvailableFromDate { get; set; }
        public DateTime AvailableToDate { get; set; }    
        public ProductCategory Category { get; set; }
        public decimal TotalPrice {
            get
            {
                return this.BasePrice + GSTAmount + PSTAmount + RecyclingFee;
            }
        }

        public decimal GSTAmount {
            get
            {
                if (this.IsGSTExempt)
                {
                    return 0;
                } else
                {
                    return ((this.BasePrice + this.RecyclingFee) * Tax.GST);
                }

            }
        }

        public decimal PSTAmount
        {
            get
            {
                if (this.IsPSTExempt)
                {
                    return 0;
                } else
                {
                    return (this.BasePrice * Tax.GST);
                }
            }
        }

    }
}
