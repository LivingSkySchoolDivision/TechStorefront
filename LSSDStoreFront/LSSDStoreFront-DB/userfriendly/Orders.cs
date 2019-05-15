using LSSD.StoreFront.Lib;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.DB.userfriendly
{
    public class Orders
    {
        public IReadOnlyList<Order> OrderHistory
        {
            get
            {
                return new List<Order>() { };
            }
        }

        public void PlaceNewOrder(User user, BudgetAccountNumber accountNumber, List<ShoppingCartItem> items)
        {
            // Create a new order object
        }





    }
}
