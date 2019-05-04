using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.Lib
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int OwnerID { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime Expired { get; set; }
    }
}
