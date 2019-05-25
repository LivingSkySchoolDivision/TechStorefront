using LSSD.StoreFront.Lib;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LSSDStoreFront_Tests.Lib_Tests
{
    public class ProductCategory_Tests
    {

        [Fact] 
        public void ProductCategory_ShouldBeTopLevelIfNoParent()
        {
            ProductCategory category = new ProductCategory()
            {
                ParentCategoryId = 0
            };

            Assert.True(category.IsTopLevel);
        }

        [Fact]
        public void ProductCategory_ShouldNotBeTopLevelIfHasParent()
        {
            ProductCategory category = new ProductCategory()
            {
                ParentCategoryId = 1
            };

            Assert.False(category.IsTopLevel);
        }

        [Fact]
        public void ProductCategory_ShouldIndicateChildrenIfHasChildren()
        {
            ProductCategory category = new ProductCategory()
            {
                ChildCategories = new List<ProductCategory>()
                {
                    new ProductCategory(),
                    new ProductCategory()
                }
            };

            Assert.True(category.HasChildren);
        }

        [Fact]
        public void ProductCategory_ShouldNotIndicateChildrenIfHasNoChildren()
        {
            ProductCategory category = new ProductCategory();

            Assert.False(category.HasChildren);
        }
    }
}
