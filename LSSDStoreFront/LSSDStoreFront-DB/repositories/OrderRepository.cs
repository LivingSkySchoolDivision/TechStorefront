using LSSD.StoreFront.Lib;
using LSSD.StoreFront.Lib.UserAccounts;
using LSSD.StoreFront.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using LSSD.StoreFront.Lib.Extensions;
using System.Collections.Concurrent;

namespace LSSD.StoreFront.DB.repositories
{
    public class OrderRepository
    {
        private readonly DatabaseContext _dbConnection;
        private readonly OrderItemRepository _orderItemRepository;
        private readonly OrderStatusDetailRepository _orderStatusDetailRepository;

        public OrderRepository(DatabaseContext DatabaseContext)
        {
            this._dbConnection = DatabaseContext;
            this._orderItemRepository = new OrderItemRepository(this._dbConnection);
            this._orderStatusDetailRepository = new OrderStatusDetailRepository(this._dbConnection);
        }

        private Order dataReaderToObject(SqlDataReader dataReader)
        {
            string OrderThumbprint = dataReader["OrderThumbprint"].ToString();

            List<OrderItem> items = _orderItemRepository.GetForOrder(OrderThumbprint);
            List<OrderStatusDetail> statusDetails = _orderStatusDetailRepository.GetForOrder(OrderThumbprint);

            return new Order()
            {
                OrderThumbprint = OrderThumbprint,
                UserThumbprint = dataReader["UserThumbprint"].ToString(),
                OrderDate = dataReader["OrderDate"].ToString().ToDateTime(),
                CustomerFullName = dataReader["SubmittedByFullName"].ToString(),
                CustomerEmailAddress = dataReader["SubmittedByEmailAddress"].ToString(),
                BudgetAccountNumber = dataReader["BudgetAccountNumber"].ToString(),
                CustomerNotes = dataReader["CustomerNotes"].ToString(),
                ManagerNotes = dataReader["ManagerNotes"].ToString(),
                OrderTotalItems = dataReader["OrderTotalItems"].ToString().ToInt(),
                Items = items,
                StatusDetails = statusDetails,
                OrderGrandTotal = dataReader["OrderGrandTotal"].ToString().ToDecimal(),
                OrderSubTotal = dataReader["OrderSubTotal"].ToString().ToDecimal(),
                TotalEHF = dataReader["TotalEHF"].ToString().ToDecimal(),
                TotalGST = dataReader["TotalGST"].ToString().ToDecimal(),
                TotalPST = dataReader["TotalPST"].ToString().ToDecimal(),
                IsCompletelyFulfilled = dataReader["IsCompletelyFulfilled"].ToString().ToBool()
            };
        }

        public List<Order> GetAll()
        {
            List<Order> returnMe = new List<Order>();

            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM Orders;"
                })
                {
                    sqlCommand.Connection.Open();
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            Order obj = dataReaderToObject(dbDataReader);
                            if (obj != null)
                            {
                                returnMe.Add(obj);
                            }
                        }
                    }

                    sqlCommand.Connection.Close();
                }
            }

            return returnMe;
        }
                
        public List<Order> GetIncomplete()
        {
            List<Order> returnMe = new List<Order>();

            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM Orders WHERE IsCompletelyFulfilled=0;"
                })
                {
                    sqlCommand.Connection.Open();
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            Order obj = dataReaderToObject(dbDataReader);
                            if (obj != null)
                            {
                                returnMe.Add(obj);
                            }
                        }
                    }

                    sqlCommand.Connection.Close();
                }
            }

            return returnMe;
        }

        public Order Get(string orderThumb)
        {
            List<Order> foundOrders = Get(new List<string>() { orderThumb });
            if (foundOrders.Count > 0)
            {
                return foundOrders[0];
            }

            return null;
        }

        public List<Order> Get(List<string> orderThumbs)
        {
            List<Order> returnMe = new List<Order>();

            if (orderThumbs.Count > 0)
            {
                StringBuilder orderThumbsParameter = new StringBuilder();
                foreach (string s in orderThumbs)
                {
                    orderThumbsParameter.Append("'" + s + "',");
                }

                // Remove the final comma
                if (orderThumbsParameter.Length > 1)
                {
                    orderThumbsParameter.Remove(orderThumbsParameter.Length - 1, 1);
                }

                using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = "SELECT * FROM Orders WHERE OrderThumbprint in ( " + orderThumbsParameter.ToString() + " );"
                    })
                    {
                        sqlCommand.Connection.Open();
                        SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                        if (dbDataReader.HasRows)
                        {
                            while (dbDataReader.Read())
                            {
                                Order obj = dataReaderToObject(dbDataReader);
                                if (obj != null)
                                {
                                    returnMe.Add(obj);
                                }
                            }
                        }

                        sqlCommand.Connection.Close();
                    }
                }
            }
            return returnMe;
        }


        public List<Order> GetForUser(UserThumbprint UserThumbprint)
        {
            List<Order> returnMe = new List<Order>();

            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM Orders WHERE UserThumbprint=@USERID"
                })
                {
                    sqlCommand.Parameters.AddWithValue("USERID", UserThumbprint.Value);
                    sqlCommand.Connection.Open();
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            Order obj = dataReaderToObject(dbDataReader);
                            if (obj != null)
                            {
                                returnMe.Add(obj);
                            }
                        }
                    }

                    sqlCommand.Connection.Close();
                }
            }

            return returnMe;
        }

        public void MarkFulfilled(Order order)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "UPDATE Orders SET IsCompletelyFulfilled=1 WHERE OrderThumbprint=@OTP"
                })
                {
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Parameters.AddWithValue("@OTP", order.OrderThumbprint);
                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
        }

        public void MarkUnfulfilled(Order order)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "UPDATE Orders SET IsCompletelyFulfilled=0 WHERE OrderThumbprint=@OTP"
                })
                {
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Parameters.AddWithValue("@OTP", order.OrderThumbprint);
                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
        }

        private bool DoesThumbprintAlreadyExist(string thumbprint)
        {
            bool result = false;
            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT OrderThumbprint FROM Orders WHERE OrderThumbprint=@ORDERID;"
                })
                {
                    sqlCommand.Parameters.AddWithValue("ORDERID", thumbprint);
                    sqlCommand.Connection.Open();
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();
                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            result = true;
                            break;
                        }
                    }
                    sqlCommand.Connection.Close();
                }
            }

            return result;
        }

        private static object _lock;

        public string Create(Order order)
        {
            string orderThumbprint = Crypto.Hash(Environment.MachineName + ":" + order.UserThumbprint + ":" + order.BudgetAccountNumber + ":" + DateTime.Now.ToLongDateString() + ":" + DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond);

            lock (_lock)
            {                
                // Make sure we don't already have an order with this exact thumbprint. If we do, the user double clicked.
                // It still might be possible (though incredibly unlikely) that the orders were intentional, so rather than
                // remove an order, just submit the order twice twith two different IDs. Then it's easier to spot and clean up

                int justANumber = 0;
                while (DoesThumbprintAlreadyExist(orderThumbprint))
                {
                    orderThumbprint = Crypto.Hash(orderThumbprint +":" + DateTime.Now.ToLongDateString() + ":" + DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond + ":" + justANumber);
                    justANumber++;
                } 

                using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = "INSERT INTO Orders(OrderThumbprint, UserThumbprint, OrderDate, SubmittedByFullName,SubmittedByEmailAddress, BudgetAccountNumber, OrderGrandTotal, CustomerNotes, ManagerNotes, OrderTotalItems, OrderSubTotal, TotalGST, TotalPST, TotalEHF, ServerName) " +
                                                    "VALUES(@OTP,@UTP,GETDATE(),@SUBMITTEDBY,@SUBMITTEDBYEMAIL,@BUDGETNUM, @GRANDTOTAL, @CUSTNOTES, @MANNOTES, @NUMITEMS, @SUBTOTAL, @TOTGST, @TOTPST, @TOTEHF, @SRVRNAME)"
                    })
                    {
                        sqlCommand.Parameters.Clear();
                        sqlCommand.Parameters.AddWithValue("@OTP", orderThumbprint);
                        sqlCommand.Parameters.AddWithValue("@UTP", order.UserThumbprint);
                        sqlCommand.Parameters.AddWithValue("@SUBMITTEDBY", order.CustomerFullName);
                        sqlCommand.Parameters.AddWithValue("@SUBMITTEDBYEMAIL", order.CustomerEmailAddress);
                        sqlCommand.Parameters.AddWithValue("@BUDGETNUM", order.BudgetAccountNumber);
                        sqlCommand.Parameters.AddWithValue("@GRANDTOTAL", order.OrderGrandTotal);
                        sqlCommand.Parameters.AddWithValue("@CUSTNOTES", order.CustomerNotes ?? string.Empty);
                        sqlCommand.Parameters.AddWithValue("@MANNOTES", order.ManagerNotes ?? string.Empty);
                        sqlCommand.Parameters.AddWithValue("@NUMITEMS", order.OrderTotalItems);
                        sqlCommand.Parameters.AddWithValue("@SUBTOTAL", order.OrderSubTotal);
                        sqlCommand.Parameters.AddWithValue("@TOTGST", order.TotalGST);
                        sqlCommand.Parameters.AddWithValue("@TOTPST", order.TotalPST);
                        sqlCommand.Parameters.AddWithValue("@TOTEHF", order.TotalEHF);
                        sqlCommand.Parameters.AddWithValue("@SRVRNAME", Environment.MachineName);
                        sqlCommand.Connection.Open();
                        sqlCommand.ExecuteNonQuery();
                        sqlCommand.Connection.Close();
                    }
                }

            }

            return orderThumbprint;
        }
        

    }
}
