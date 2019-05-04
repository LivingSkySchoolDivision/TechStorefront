using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.Lib
{
    public class BudgetAccountNumber
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string AccountNumber { get; set; }
        public int OwnerID { get; set; }        
    }
}
