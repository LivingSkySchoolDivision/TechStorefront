﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSSD.StoreFront.DB
{
    public static class ParserExtensions
    {
        // Date bounds of Microsoft SQL server
        private static readonly DateTime dbMinDate = new DateTime(1753, 01, 01);
        private static readonly DateTime dbMaxDate = new DateTime(1999, 12, 31);

        /// <summary>
        /// Sanitize a DateTime object to be put in an MS SQL database. Microsoft SQL server's minumum and maximum dates are 
        /// different than C#, so this method adjusts the provided DateTime object to match the SQL min/max.
        /// </summary>
        /// <param name="unsafeDateTime">DateTime that may be out of bounds</param>
        /// <returns>DateTime object that is safe to use in MS SQL</returns>
        public static DateTime ToDatabaseSafeDateTime(this DateTime unsafeDateTime)
        {
            if (unsafeDateTime > dbMaxDate)
            {
                unsafeDateTime = dbMaxDate;
            }

            if (unsafeDateTime < dbMinDate)
            {
                unsafeDateTime = dbMinDate;
            }

            return unsafeDateTime;
        }

        /// <summary>
        /// Parses a string to a Decimal
        /// </summary>
        /// <param name="thisString"></param>
        /// <returns></returns>
        public  static decimal ToDecimal(this string thisString)
        {
            if (decimal.TryParse(thisString, out decimal result))
            {
                return result;
            }
            return (decimal)0.00;
        }

        /// <summary>
        /// Parses a string to an Integer. Returns 0 on failure.
        /// </summary>
        /// <param name="thisString"></param>
        /// <returns></returns>
        public static int ToInt(this string thisString)
        {
            int returnMe = 0;

            if (Int32.TryParse(thisString, out returnMe))
            {
                return returnMe;
            }

            return 0;
        }

        /// <summary>
        /// Parses a string to a Double. Returns 0 on failure.
        /// </summary>
        /// <param name="thisString"></param>
        /// <returns></returns>
        public static double ToDouble(this string thisString)
        {
            Double returnMe = 0;

            if (Double.TryParse(thisString, out returnMe))
            {
                return returnMe;
            }

            return (Double)0;
        }

        /// <summary>
        /// Parses a string to a DateTime. Returns DateTime.MinValue on failure.
        /// </summary>
        /// <param name="thisDate"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string thisDate)
        {
            DateTime returnMe = DateTime.MinValue;

            if (!DateTime.TryParse(thisDate, out returnMe))
            {
                returnMe = DateTime.MinValue;
            }

            return returnMe;
        }

        /// <summary>
        /// Parses a string to a bool. Always returns a value, and returns false on a failure.
        /// </summary>
        /// <param name="thisDatabaseValue"></param>
        /// <returns></returns>
        public static bool ToBool(this string thisDatabaseValue)
        {
            if (String.IsNullOrEmpty(thisDatabaseValue))
            {
                return false;
            }
            else
            {
                bool parsedBool = false;
                Boolean.TryParse(thisDatabaseValue, out parsedBool);
                return parsedBool;
            }
        }

    }
}
