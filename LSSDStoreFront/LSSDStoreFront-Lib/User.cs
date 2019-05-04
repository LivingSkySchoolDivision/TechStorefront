using System;
using System.Collections.Generic;
using System.Text;

namespace LSSDStoreFront_Lib
{
    class User
    {
        public string LDAPUsername { get; set; }
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsManager { get; set; }
        public bool IsAdministrator { get; set; }
        public bool CanImpersinate { get; set; }        
    }
}
