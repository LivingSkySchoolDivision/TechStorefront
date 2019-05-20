using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LSSD.StoreFront.FrontEnd.Extensions
{
    public static class HTMLHelperExtensions
    {
        public static string ToYesOrNo(this bool value)
        {
            return value ? "Yes" : "No";
        }
    }
}
