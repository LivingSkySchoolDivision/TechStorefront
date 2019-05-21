using LSSD.StoreFront.Lib;
using LSSD.StoreFront.Lib.UserAccounts;
using LSSD.StoreFront.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

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
                SubmittedBy = dataReader["SubmittedByFullName"].ToString(),
                BudgetAccountNumber = dataReader["BudgetAccountNumber"].ToString(),
                OrderGrandTotal = dataReader["OrderGrandTotal"].ToString().ToDecimal(),
                CustomerNotes = dataReader["CustomerNotes"].ToString(),
                ManagerNotes = dataReader["ManagerNotes"].ToString(),
                OrderTotalItems = dataReader["OrderTotalItems"].ToString().ToInt(),
                Items = items,
                StatusDetails = statusDetails
            };
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

        public string Create(Order order)
        {
            string orderThumbprint = Crypto.Hash(order.UserThumbprint + ":" + order.BudgetAccountNumber + ":" + DateTime.Now.ToLongDateString() + ":" + DateTime.Now.ToLongTimeString());

            // Calculate some fields regardless of what was sent to this method
            int numItems = order.Items.Sum(x => x.Quantity);
            decimal orderGrandTotal = order.Items.Sum(x => (x.ItemPrice * x.Quantity));

            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "INSERT INTO Orders(OrderThumbprint, UserThumbprint, OrderDate, SubmittedByFullName, BudgetAccountNumber, OrderGrandTotal, CustomerNotes, ManagerNotes, OrderTotalItems) " +
                                                "VALUES(@OTP,@UTP,@ODATE,@SUBMITTEDBY,@BUDGETNUM, @GRANDTOTAL, @CUSTNOTES, @MANNOTES, @NUMITEMS)"
                })
                {
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Parameters.AddWithValue("@OTP", orderThumbprint);
                    sqlCommand.Parameters.AddWithValue("@UTP", order.UserThumbprint);
                    sqlCommand.Parameters.AddWithValue("@ODATE", DateTime.Now);
                    sqlCommand.Parameters.AddWithValue("@SUBMITTEDBY", order.SubmittedBy);
                    sqlCommand.Parameters.AddWithValue("@BUDGETNUM", order.BudgetAccountNumber);
                    sqlCommand.Parameters.AddWithValue("@GRANDTOTAL", orderGrandTotal);
                    sqlCommand.Parameters.AddWithValue("@CUSTNOTES", order.CustomerNotes ?? string.Empty);
                    sqlCommand.Parameters.AddWithValue("@MANNOTES", order.ManagerNotes ?? string.Empty);
                    sqlCommand.Parameters.AddWithValue("@NUMITEMS", numItems);
                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }

            return orderThumbprint;
        }
        

    }
}
