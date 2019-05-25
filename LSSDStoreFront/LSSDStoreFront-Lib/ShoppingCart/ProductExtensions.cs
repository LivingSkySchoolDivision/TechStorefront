using LSSD.StoreFront.Lib.UserAccounts;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.Lib
{
    public static class ProductExtensions
    {

        public static ShoppingCartItem ToShoppingCartItem(this Product Product, UserThumbprint userthumbprint)
        {
            return ToShoppingCartItem(Product, userthumbprint, 1);
        }

        public static ShoppingCartItem ToShoppingCartItem(this Product Product, UserThumbprint userthumbprint, int Quantity)
        {
            // I don't this situation can actually happen, but just in case I'm mistaken
            if (Product == null)
            {
                throw new ProductConversionException("Given product object was null");
            }

            if (userthumbprint == null)
            {
                throw new ProductConversionException("Given user thumbprint was null");
            }

            if (string.IsNullOrEmpty(userthumbprint.Value))
            {
                throw new ProductConversionException("User thumbprint was empty");
            }

            return new ShoppingCartItem()
            {
                ProductId = Product.Id,
                Product = Product,
                UserThumbprint = userthumbprint.Value,
                Quantity = Quantity
            };
                
        }
    }
}
