using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.Lib.Products
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public byte[] ImageBytes { get; set; }
        public string ContentType { get; set; }
    }
}
