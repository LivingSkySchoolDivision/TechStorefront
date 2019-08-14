using LSSD.StoreFront.Lib;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.EmailRunner.Models
{
    class EmailNotification
    {
        public string To { get; set; }
        public Order Order { get; set; }
        public CannedEmailMessage Message { get; set; }
    }
}
