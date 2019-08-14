using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.Lib.Orders
{
    public class OrderNotificationLogEntry
    {
        public string OrderThumbPrint { get; set; }
        public DateTime NotificationDate { get; set; }
        public string Recipient { get; set; }
        public bool WasSentWithoutError { get; set; }
        public bool WasToCustomer { get; set; }
        public bool WasToOrderDesk { get; set; }
        public string Notes { get; set; }
    }
}
