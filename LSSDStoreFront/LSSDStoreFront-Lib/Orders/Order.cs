using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSSD.StoreFront.Lib
{
    public class Order
    {
        public string OrderThumbprint{ get; set; }
        public string UserThumbprint { get; set; } 
        public DateTime OrderDate { get; set; }
        public string SubmittedBy { get; set; }
        public string BudgetAccountNumber { get; set; }
        public decimal OrderGrandTotal { get; set; }
        public int OrderTotalItems { get; set; }
        public string CustomerNotes { get; set; }
        public string ManagerNotes { get; set; }
        public List<OrderItem> Items { get; set; }
        public List<OrderStatusDetail> StatusDetails { get; set; }

        public Order()
        {
            this.Items = new List<OrderItem>();
            this.StatusDetails = new List<OrderStatusDetail>();
        }


        public string LastKnownStatus
        {
            get
            {
                if (this.StatusDetails.Count == 0)
                {
                    return "Unknown";
                }
                else
                {
                    return this.StatusDetails.OrderByDescending(x => x.Timestamp).First().Status;
                }
            }
        }
    }
}
