using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.Lib
{
    public static class ProductExtensions
    {

        public static ShoppingCartItem ToShoppingCartItem(this Product Product, User user)
        {
            return ToShoppingCartItem(Product, user, 1);
        }

        public static ShoppingCartItem ToShoppingCartItem(this Product Product, User user, int Quantity)
        {
            if (Product != null)
            {
                if (user != null)
                {
                    return new ShoppingCartItem()
                    {
                        ProductId = Product.Id,
                        Product = Product,
                        UserId = user.Id,
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
