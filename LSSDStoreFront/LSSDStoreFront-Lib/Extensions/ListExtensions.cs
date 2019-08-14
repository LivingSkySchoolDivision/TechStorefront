using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.Lib.Extensions
{
    public static class ListExtensions
    {
        public static List<T> AddRangeUnique<T>(this List<T> OriginalList, List<T> AdditionalItems)
        {            
            foreach (T obj in AdditionalItems)
            {
                if (!OriginalList.Contains(obj))
                {
                    OriginalList.Add(obj);
                }
            }
            return OriginalList;
        }

        public static string ToCommaSeparatedString<T>(this List<T> list)
        {
            StringBuilder returnMe = new StringBuilder();

            foreach (T item in list)
            {
                returnMe.Append(item);
                returnMe.Append(", ");
            }

            if (returnMe.Length > 2)
            {
                returnMe.Remove(returnMe.Length - 2, 2);
            }

            return returnMe.ToString();
        }

    }
}
