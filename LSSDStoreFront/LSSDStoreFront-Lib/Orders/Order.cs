using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.Lib
{
    public class Order
    {
        public int Id { get; set; }
        public string UserThumbprint { get; set; } 
        public DateTime OrderDate { get; set; }
        public string SubmittedBy { get; set; }
        public string BudgetAccountNumber { get; set; }
    }
}
