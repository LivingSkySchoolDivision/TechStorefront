using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.Lib.Exceptions
{
    public class InvalidUsernameException : Exception
    {
        public InvalidUsernameException() : base() { }
        public InvalidUsernameException(string message) : base(message) { }
        public InvalidUsernameException(string message, Exception ex) : base(message, ex) { }
    }
}
