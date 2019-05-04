using System;
using System.Collections.Generic;
using System.Text;

namespace LSSDStoreFront_Lib
{
    class CompletedOrder
    {
        public int OrderID { get; set; }
        public int SubmittedBy { get; set; } //Expects User ID
        public int BudgetAccountNumber { get; set; }
    }
}
