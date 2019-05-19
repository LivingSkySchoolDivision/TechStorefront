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

        /*
        public static implicit operator string(UserThumbprint obj)
        {
            return obj.Value;
        }

        public static implicit operator UserThumbprint(string str)
        {
            return new UserThumbprint()
            {
                Value = str
            };
        }
        */

        public override bool Equals(object obj)
        {
            if (obj is string)
            {
                return this.Value.Equals(Crypto.Hash((string)obj));
            }

            if (obj is UserThumbprint)
            {
                UserThumbprint objTP = (UserThumbprint)obj;
                return this.Value.Equals(objTP.Value);
            }

            return false;
        }

    }
}
