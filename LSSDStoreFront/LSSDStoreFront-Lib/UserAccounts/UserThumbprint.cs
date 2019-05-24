using LSSD.StoreFront.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.Lib.UserAccounts
{
    public class UserThumbprint
    {
        public string Value { get; private set; }

        public UserThumbprint(string Username)
        {
            this.Value = Crypto.Hash(Username);
        }

        private UserThumbprint() { }
    }
}
