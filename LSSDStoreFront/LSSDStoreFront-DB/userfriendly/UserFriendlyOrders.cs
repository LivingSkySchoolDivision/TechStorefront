﻿using LSSD.StoreFront.DB.repositories;
using LSSD.StoreFront.Lib;
using LSSD.StoreFront.Lib.UserAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using LSSD.StoreFront.Lib.Utilities;
using System.Text;

namespace LSSD.StoreFront.DB
{
    public class UserFriendlyOrders
    {
        private readonly OrderRepository _orderRepository;
        private readonly DatabaseContext _dbContext;
        private readonly UserThumbprint _userThumbPrint;

        private readonly Dictionary<string, Order> UserOrders;
        
        public UserFriendlyOrders(DatabaseContext dbContext, string UserAccount)
        {
            this._dbContext = dbContext;
            this._orderRepository = new OrderRepository(this._dbContext);
            this._userThumbPrint = new UserThumbprint(UserAccount);
            this.UserOrders = new Dictionary<string, Order>();

            foreach(Order order in _orderRepository.GetForUser(this._userThumbPrint))
            {
                if (!UserOrders.ContainsKey(order.OrderThumbprint))
                {
                    UserOrders.Add(order.OrderThumbprint, order);
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

        public Order Get(string Thumbprint)
        {
            if (UserOrders.ContainsKey(Thumbprint))
            {
                return UserOrders[Thumbprint];
            }
            return null;
        }

        public Order CreateOrder(IEnumerable<ShoppingCartItem> items, string BudgetAccountNumber, string SubmittedByName, string SubmittdByEmail, UserThumbprint userThumbprint, string customerNotes)
        {
            OrderStatusDetailRepository orderStatusDetailRepository = new OrderStatusDetailRepository(this._dbContext);
            OrderItemRepository orderItemRepository = new OrderItemRepository(this._dbContext);

            // We need to get a new OrderID or make one somehow
            // Hash the user thumbprint and the date and time together, and it should be fairly unique...
            
            List<OrderItem> newOrderItems = new List<OrderItem>();
            foreach(ShoppingCartItem scitem in items)
            {
                if (scitem.Quantity > 0)
                {
                    newOrderItems.Add(new OrderItem()
                    {
                        OrderThumbprint = "WILL-GET-REPLACED",
                        Name = scitem.Product.Name,
                        ItemBasePrice = scitem.Product.BasePrice,
                        ItemGST = scitem.Product.GSTAmount,
                        ItemPST = scitem.Product.PSTAmount,
                        ItemEHF = scitem.Product.RecyclingFee,
                        ItemPriceWithTax = scitem.Product.TotalPrice,
                        TotalBasePrice = (decimal)(scitem.Product.BasePrice * scitem.Quantity),
                        TotalEHF = (decimal)(scitem.Product.RecyclingFee * scitem.Quantity),
                        TotalPST = (decimal)(scitem.Product.PSTAmount * scitem.Quantity),
                        TotalGST = (decimal)(scitem.Product.GSTAmount * scitem.Quantity),
                        TotalPriceWithTax = (decimal)(scitem.Product.TotalPrice * scitem.Quantity),
                        ProductId = scitem.ProductId,
                        Quantity = scitem.Quantity,
                    });
                }
            }
            
            OrderStatusDetail newOrderStatusDetail = new OrderStatusDetail()
            {
                OrderThumbprint = "WILL-GET-REPLACED",
                Status = "Order Submitted",
                Timestamp = DateTime.Now,
                UpdatedBy = SubmittedByName,
                Notes = string.Empty
            };


            Order newOrder = new Order()
            {
                OrderThumbprint = "WILL-GET-REPLACED",
                UserThumbprint = userThumbprint.Value,
                OrderDate = DateTime.Now,
                CustomerFullName = SubmittedByName,
                CustomerEmailAddress = SubmittdByEmail,
                BudgetAccountNumber = BudgetAccountNumber,
                CustomerNotes = customerNotes,
                StatusDetails = new List<OrderStatusDetail>() { newOrderStatusDetail },
                Items = newOrderItems,
                OrderTotalItems = newOrderItems.Sum(x => x.Quantity),
                OrderSubTotal = newOrderItems.Sum(x => x.TotalBasePrice),
                OrderGrandTotal = newOrderItems.Sum(x => x.TotalPriceWithTax),
                TotalEHF = newOrderItems.Sum(x => x.TotalEHF),
                TotalGST = newOrderItems.Sum(x => x.TotalGST),
                TotalPST = newOrderItems.Sum(x => x.TotalPST)
            };

            string orderThumbprint = _orderRepository.Create(newOrder);

            // Update the order thumbprint for things that need it
            newOrder.OrderThumbprint = orderThumbprint;
            newOrder.StatusDetails.ForEach(x => x.OrderThumbprint = orderThumbprint);
            newOrder.Items.ForEach(x => x.OrderThumbprint = orderThumbprint);

            orderItemRepository.Create(newOrder.Items);
            orderStatusDetailRepository.Create(newOrder.StatusDetails);

            return newOrder;
        }
    }
}
