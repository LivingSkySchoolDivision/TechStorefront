using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.DB
{
    public class DatabaseContext
    {
        public string ConnectionString { get; set; }


        public DatabaseContext(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }

    }
}
