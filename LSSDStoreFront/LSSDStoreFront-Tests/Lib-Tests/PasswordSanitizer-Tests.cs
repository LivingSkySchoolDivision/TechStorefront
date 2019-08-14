using System;
using System.Collections.Generic;
using System.Text;
using LSSD.StoreFront.Lib;
using Xunit;

namespace LSSDStoreFront_Tests.Lib_Tests
{
    public class PasswordSanitizer_Tests
    {
        [Theory]
        [InlineData("abc123", 6)]
        [InlineData("password", 8)]
        [InlineData("abcdefghijklmnopqrstuvwxyz", 26)]
        [InlineData("", 0)]
        [InlineData("a", 1)]
        public void PasswordSanitizerShouldReturnStringOfSameLengthAsPassword(string testString, int expectedLength)
        {
            Assert.Equal(testString.MaskPassword().Length, expectedLength);
        }

        [Fact]
        public void PasswordSanitizerShouldReturnNonEmptyString()
        {
            Assert.NotEqual(0, "abcdefg".MaskPassword().Length);
        }
    }
}
