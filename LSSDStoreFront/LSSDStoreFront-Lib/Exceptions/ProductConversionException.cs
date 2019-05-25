using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.Lib
{
    public class ProductConversionException : Exception
    {
        public ProductConversionException() : base() { }
        public ProductConversionException(string message) : base(message) { }
        public ProductConversionException(string message, Exception ex) : base(message, ex) { }
    }
}
