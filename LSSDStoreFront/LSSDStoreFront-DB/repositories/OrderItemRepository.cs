using LSSD.StoreFront.Lib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace LSSD.StoreFront.DB.repositories
{
    class OrderItemRepository
    {
        private DatabaseContext _dbConnection;
        public OrderItemRepository(DatabaseContext DatabaseConnection)
        {
            this._dbConnection = DatabaseConnection;
        }

        private OrderItem dataReaderToObject(SqlDataReader dataReader)
        {
            return new OrderItem()
            {
                Id = dataReader["Id"].ToString().ToInt(),
                OrderId = dataReader["OrderId"].ToString().ToInt(),
                Name = dataReader["ItemName"].ToString(),
                Price = dataReader["ItemPrice"].ToString().ToDecimal(),
                ProductId = dataReader["ProductId"].ToString().ToInt(),
                Fulfilled = dataReader["Fulfilled"].ToString().ToBool(),
                FulfilledDate = dataReader["FulfilledDate"].ToString().ToDateTime()
            };
        }
        
        public List<OrderItem> GetForOrder(int OrderId)
        {
            List<OrderItem> returnMe = new List<OrderItem>();

            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM OrderItems WHERE OrderId=@ORDERID"
                })
                {
                    sqlCommand.Parameters.AddWithValue("ORDERID", OrderId);
                    sqlCommand.Connection.Open();

                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            OrderItem product = dataReaderToObject(dbDataReader);

                            if (product != null)
                            {
                                returnMe.Add(product);
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
