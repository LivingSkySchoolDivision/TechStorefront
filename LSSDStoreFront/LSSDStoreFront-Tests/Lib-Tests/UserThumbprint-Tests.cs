using LSSD.StoreFront.Lib.Exceptions;
using LSSD.StoreFront.Lib.UserAccounts;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LSSDStoreFront_Tests.Lib_Tests
{
    public class UserThumbprint_Tests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("1")]
        [InlineData("test")]
        [InlineData("dDlXjNK4MQ1DT2mAQLpK8creeorXrOmpRZihExNUCbsor9KZ6gLnb45amgT5vXM8")]
        public void UserThumbprint_HasValue(string input)
        {
            UserThumbprint thumb = new UserThumbprint(input);
            Assert.NotEmpty(thumb.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("1")]
        [InlineData("test")]
        [InlineData("dDlXjNK4MQ1DT2mAQLpK8creeorXrOmpRZihExNUCbsor9KZ6gLnb45amgT5vXM8")]
        public void UserThumbprint_CreatesConsistentValues(string input)
        {
            UserThumbprint thumb1 = new UserThumbprint(input);
            UserThumbprint thumb2 = new UserThumbprint(input);

            Assert.Equal(thumb1.Value, thumb2.Value);
        }

        [Fact]
        public void UserThumbprint_ShouldThrowExceptionOnNullInput()
        {
            string input = null;
            UserThumbprint thumb;
            Assert.Throws<InvalidUsernameException>(() => thumb = new UserThumbprint(input));
        }
    }
}
