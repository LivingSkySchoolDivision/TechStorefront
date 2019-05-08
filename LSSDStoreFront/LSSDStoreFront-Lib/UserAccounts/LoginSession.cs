using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.Lib
{
    public class LoginSession
    {
        public string Username { get; set; }
        public string IPAddress { get; set; }
        public string Thumbprint { get; set; }
        public string BrowserUserAgent { get; set; }
        public DateTime Starts { get; set; }
        public DateTime Ends { get; set; }        
    }
}
