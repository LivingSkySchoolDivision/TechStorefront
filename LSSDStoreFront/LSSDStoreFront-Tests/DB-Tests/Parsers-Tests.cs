using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using LSSD.StoreFront.DB;

namespace LSSDStoreFront_Tests.DB_Tests
{
    public class Parsers_Tests
    {
        [Fact]
        public void ToDatabaseSafeDateTime_ShouldCorrectDateTooSmall()
        {
            DateTime minimumDateForSQL = new DateTime(1753, 01, 01);
            DateTime testDate = DateTime.MinValue;
            Assert.True(testDate.ToDatabaseSafeDateTime() == minimumDateForSQL);
        }

        [Fact]
        public void ToDatabaseSafeDateTime_ShouldCorrectDateTooLarge()
        {
            DateTime maximumDateForSQL = new DateTime(9999, 12, 31);
            DateTime testDate = DateTime.MaxValue;
            Assert.True(testDate.ToDatabaseSafeDateTime() == maximumDateForSQL);
        }

        [Fact]
        public void ToDatabaseSafeDateTime_ShouldNotAlterValidDate()
        {
            DateTime testDate = DateTime.Now;
            DateTime controlDate = testDate;

            Assert.True(testDate.ToDatabaseSafeDateTime() == controlDate);
        }

        [Theory]
        [InlineData("1", 1)]
        [InlineData("10", 10)]
        [InlineData("1.1", 1.1)]
        [InlineData("12345678.9", 12345678.9)]
        [InlineData("-12345678.9", -12345678.9)]
        [InlineData("123.456789", 123.456789)]
        [InlineData(".1", 0.1)]
        public void ToDecimal_ShouldParseValidDecimalValues(string given, decimal expected)
        {
            Assert.Equal(given.ToDecimal(), expected);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("abc")]
        [InlineData("abc.def")]
        [InlineData("123.456.789")]
        [InlineData("1a2.b3c")]
        public void ToDecimal_ShouldReturnZeroOnInvalidValue(string given)
        {
            Assert.True(given.ToDecimal() == (decimal)0);
        }

        [Theory]
        [InlineData("1", 1)]
        [InlineData("123", 123)]
        [InlineData("1234", 1234)]
        [InlineData("-1", -1)]
        [InlineData("-2147483648", -2147483648)]
        [InlineData("2147483647", 2147483647)]
        public void ToInt_ShouldParseValidDecimalValues(string given, int expected)
        {
            Assert.Equal(given.ToInt(), expected);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("abc")]
        [InlineData("true")]
        [InlineData("false")]
        [InlineData("abc.def")]
        [InlineData("123.456.789")]
        [InlineData("1.1")]
        [InlineData("123.456")]
        public void ToInt_ShouldReturnZeroOnInvalidValue(string given)
        {
            Assert.True(given.ToInt() == (int)0);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("true")]
        [InlineData("True")]
        [InlineData("TRUE")]
        [InlineData("TrUe")]
        [InlineData("   true   ")]
        public void ToBool_ShouldParseCommonTrueValues(string given)
        {
            Assert.True(given.ToBool());
        }

        [Theory]
        [InlineData("0")]
        [InlineData("false")]
        [InlineData("False")]
        [InlineData("FALSE")]
        [InlineData("FaLsE")]
        [InlineData("   false   ")]
        public void ToBool_ShouldParseCommonFalseValues(string given)
        {
            Assert.False(given.ToBool());
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("abc")]
        [InlineData("abc.def")]
        [InlineData("123.456.789")]
        [InlineData("1.1")]
        [InlineData("123.456")]
        public void ToBool_ShouldReturnFalseOnInvalidValue(string given)
        {
            Assert.False(given.ToBool());
        }

        [Theory]
        [InlineData("1", 1)]
        [InlineData("123", 123)]
        [InlineData("-1", -1)]
        [InlineData("-2147483648", -2147483648)]
        [InlineData("2147483647", 2147483647)]
        [InlineData("-2147483648.34453435445", -2147483648.34453435445)]
        [InlineData("2147483648.34453435445", 2147483648.34453435445)]
        public void ToDouble_ShouldParseValidDoubleValues(string given, double expected)
        {
            Assert.Equal(given.ToDouble(), expected);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("abc")]
        [InlineData("true")]
        [InlineData("false")]
        [InlineData("abc.def")]
        [InlineData("123.456.789")]
        public void ToDouble_ShouldReturnZeroOnInvalidValue(string given)
        {
            Assert.True(given.ToDouble() == (int)0);
        }

        [Theory]
        [InlineData(@"1/1/1")]
        [InlineData(@"05/01/2009")]
        [InlineData(@"05/01/2009 14:57:32.8")]
        [InlineData(@"2009-05-01 14:57:32.8")]
        [InlineData(@"2009-05-01T14:57:32.8375298-04:00")]
        [InlineData(@"Fri, 15 May 2009 20:10:57 GMT")]
        public void ToDateTime_ShouldParseValidDateValues(string given)
        {
            Assert.True(given.ToDateTime() > DateTime.MinValue);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("abc")]
        [InlineData("true")]
        [InlineData("false")]
        [InlineData("abc.def")]
        [InlineData("123.456.789")]
        public void ToDateTime_ShouldReturnMinValueOnInvalidValue(string given)
        {
            Assert.Equal(given.ToDateTime(), DateTime.MinValue);
        }






    }
}
