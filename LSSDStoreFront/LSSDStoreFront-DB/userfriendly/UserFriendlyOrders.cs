using LSSD.StoreFront.DB.repositories;
using LSSD.StoreFront.Lib;
using LSSD.StoreFront.Lib.UserAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSSD.StoreFront.DB
{
    public class UserFriendlyOrders
    {
        private readonly OrderRepository _orderRepository;
        private readonly DatabaseContext _dbContext;
        private readonly UserThumbprint _userThumbPrint;

        private Dictionary<int, Order> UserOrders;
        
        public UserFriendlyOrders(DatabaseContext dbContext, string UserAccount)
        {
            this._dbContext = dbContext;
            this._orderRepository = new OrderRepository(this._dbContext);
            this._userThumbPrint = new UserThumbprint(UserAccount);
            this.UserOrders = new Dictionary<int, Order>();

            foreach(Order order in _orderRepository.GetForUser(this._userThumbPrint))
            {
                if (!UserOrders.ContainsKey(order.Id))
                {
                    UserOrders.Add(order.Id, order);
                }
            }
        }

        public IReadOnlyList<Order> OrderHistory
        {
            get
            {
                return UserOrders.Values.OrderByDescending(x => x.OrderDate).ToList();
            }
        }

        public Order Get(int OrderId)
        {
            if (UserOrders.ContainsKey(OrderId))
            {
                return UserOrders[OrderId];
            }
            return null;
        }
    }
}
