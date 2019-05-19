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
            if (Product != null)
            {
                if (!string.IsNullOrEmpty(userthumbprint.Value))
                {
                    return new ShoppingCartItem()
                    {
                        ProductId = Product.Id,
                        Product = Product,
                        UserThumbprint = userthumbprint.Value,
                        Quantity = Quantity
                    };
                } else
                {
                    throw new Exception("Cannot convert to shopping cart item because user object was null");
                }
            } else
            {
                throw new Exception("Cannot convert to shopping cart item because product object was null");
            }
            
        }
    }
}
