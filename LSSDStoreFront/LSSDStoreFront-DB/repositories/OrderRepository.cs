using LSSD.StoreFront.Lib;
using LSSD.StoreFront.Lib.UserAccounts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace LSSD.StoreFront.DB.repositories
{
    public class OrderRepository
    {
        private DatabaseContext _dbConnection;
        private OrderItemRepository _orderItemRepository;
        private OrderStatusDetailRepository _orderStatusDetailRepository;

        public OrderRepository(DatabaseContext DatabaseContext)
        {
            this._dbConnection = DatabaseContext;
            this._orderItemRepository = new OrderItemRepository(this._dbConnection);
            this._orderStatusDetailRepository = new OrderStatusDetailRepository(this._dbConnection);
        }

        private Order dataReaderToObject(SqlDataReader dataReader)
        {
            int orderId = dataReader["Id"].ToString().ToInt();

            List<OrderItem> items = _orderItemRepository.GetForOrder(orderId);
            List<OrderStatusDetail> statusDetails = _orderStatusDetailRepository.GetForOrder(orderId);

            return new Order()
            {
                Id = orderId,
                OrderNumber = dataReader["OrderNumber"].ToString(),
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
        

    }
}
