using LSSD.StoreFront.Lib;
using LSSD.StoreFront.Lib.UserAccounts;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.DB.Orders
{
    public class UserFriendlyOrders
    {
        public IReadOnlyList<Order> OrderHistory
        {
            get
            {
                return new List<Order>() { };
            }
        }

        public void PlaceNewOrder(UserThumbprint UserThumbprint, BudgetAccountNumber AccountNumber, List<ShoppingCartItem> Items, string Notes)
        {
            // Create a new order object
        }

    }
}
