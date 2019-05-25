using LSSD.StoreFront.Lib;
using LSSD.StoreFront.Lib.UserAccounts;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LSSDStoreFront_Tests.Lib_Tests
{
    public class ProductExtensions_Tests
    {
        private readonly UserThumbprint testThumbprint = new UserThumbprint("test");
        private readonly Product testProduct = new Product()
        {
            Id = 1
        };

        [Fact]
        public void ProuctExtensions_ProductShouldConvertToShoppingCart()
        {
            Assert.IsType<ShoppingCartItem>(testProduct.ToShoppingCartItem(testThumbprint, 1));
        }

        [Theory]
        [InlineData(12)]
        [InlineData(999)]
        [InlineData(1)]
        [InlineData(0)]
        [InlineData(-1)]
        public void ProductExtensions_ShoppingCartItemShouldHaveAccurateQuantity(int quantity)
        {
            Assert.Equal(testProduct.ToShoppingCartItem(testThumbprint, quantity).Quantity, quantity);
        }

        [Fact]
        public void ProductExtensions_ShoppingCartItemShouldIncludeUserThumbprint()
        {
            Assert.Equal(testProduct.ToShoppingCartItem(testThumbprint, 1).UserThumbprint, testThumbprint.Value);
        }

        [Theory]
        [InlineData(12)]
        [InlineData(999)]
        [InlineData(1)]
        [InlineData(0)]
        [InlineData(-1)]
        public void ProductExtensions_ShoppingCartItemShouldIncludeValidProductId(int productId)
        {
            Product product = new Product() { Id = productId };
            Assert.Equal(product.ToShoppingCartItem(testThumbprint, 1).ProductId, productId);
        }

        [Fact]
        public void ProductExtensions_ShouldThrowExceptionIfUserThumbIsNull()
        {
            UserThumbprint nullUser = null;
            Assert.Throws<ProductConversionException>(() => testProduct.ToShoppingCartItem(nullUser));
        }
    }
}
