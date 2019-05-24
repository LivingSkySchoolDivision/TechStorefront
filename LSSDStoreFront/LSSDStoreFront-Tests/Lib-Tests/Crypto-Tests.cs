using LSSD.StoreFront.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LSSDStoreFront_Tests.Lib_Tests
{
    public class Crypto_Tests
    {
        [Theory(DisplayName = "Hash method should return a valid SHA256 hash")]
        [InlineData("abc123", "6ca13d52ca70c883e0f0bb101e425a89e8624de51db2d2392593af6a84118090")]
        [InlineData("muffins", "650e9595236154547665929c8ff6c06a4e1f89bb63797649b3cfc5337d075be8")]
        [InlineData("5d5b09f6dcb2d53a5fffc60c4ac0d55fabdf556069d6631545f42aa6e3500f2e", "7b4ac9b4d6a0364f5f4f1063f41f858beb5167cf3bac39a781fbb1ef44c17804")]
        [InlineData("7b4ac9b4d6a0364f5f4f1063f41f858beb5167cf3bac39a781fbb1ef44c17804", "4ededc829bc54104a004fbf1663780dca9cb5e1290991b67bf58bf067607b216")]
        public void Hash_ShouldReturnValidHash(string input, string expected)
        {
            Assert.Equal(expected, Crypto.Hash(input));
        }

        [Fact(DisplayName = "Hash method should return a SHA256 hash of an empty string")]
        public void Hash_ShouldReturnValidOnEmptyInput()
        {
            Assert.Equal("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855", Crypto.Hash(string.Empty));
        }

        [Fact(DisplayName = "Hash method should return a SHA256 hash of a null string")]
        public void Hash_ShouldReturnValidOnNullInput()
        {
            Assert.Equal("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855", Crypto.Hash(null));
        }
    }
}
