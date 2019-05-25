using LSSD.StoreFront.Lib;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LSSDStoreFront_Tests.Lib_Tests
{
    public class Product_Tests
    {

        [Fact]
        public void Product_ShouldCalculateGSTAccurately()
        {
            Product testProduct = new Product()
            {
                RecyclingFee = (decimal)1.00,
                BasePrice = (decimal)1.00,
                IsGSTExempt = false,
                IsPSTExempt = false
            };

            Assert.Equal(testProduct.GSTAmount, ((decimal)1.00 + (decimal)1.00) * (Tax.GST));
        }

        [Fact]
        public void Product_ShouldCalculatePSTAccurately()
        {
            Product testProduct = new Product()
            {
                RecyclingFee = (decimal)1.00,
                BasePrice = (decimal)1.00,
                IsGSTExempt = false,
                IsPSTExempt = false
            };
            Assert.Equal(testProduct.PSTAmount, (decimal)1.00 * (Tax.SaskPST));
        }

        [Fact]
        public void Product_ShouldCalculateTotalPriceAccurately()
        {
            Product testProduct = new Product()
            {
                RecyclingFee = (decimal)1.00,
                BasePrice = (decimal)1.00,
                IsGSTExempt = false,
                IsPSTExempt = false
            };

            decimal expected = ((decimal)1.00 + (decimal)1.00) + (((decimal)1.00 + (decimal)1.00) * Tax.GST) + ((decimal)1.00 * Tax.SaskPST);

            Assert.Equal(testProduct.TotalPrice, expected);
        }

        [Fact]
        public void Product_ShouldCalculateTotalPriceAccuratelyIfGSTExempt()
        {
            Product testProduct = new Product()
            {
                RecyclingFee = (decimal)1.00,
                BasePrice = (decimal)1.00,
                IsGSTExempt = true,
                IsPSTExempt = false
            };

            decimal expected = ((decimal)1.00 + (decimal)1.00) + ((decimal)1.00 * Tax.SaskPST);

            Assert.Equal(testProduct.TotalPrice, expected);
        }

        [Fact]
        public void Product_ShouldCalculateTotalPriceAccuratelyIfPSTExempt()
        {
            Product testProduct = new Product()
            {
                RecyclingFee = (decimal)1.00,
                BasePrice = (decimal)1.00,
                IsGSTExempt = false,
                IsPSTExempt = true
            };

            decimal expected = ((decimal)1.00 + (decimal)1.00) + (((decimal)1.00 + (decimal)1.00) * Tax.GST);

            Assert.Equal(testProduct.TotalPrice, expected);
        }
    }
}
