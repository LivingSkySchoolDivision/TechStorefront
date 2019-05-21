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
        private readonly DatabaseContext _dbConnection;
        public OrderItemRepository(DatabaseContext DatabaseConnection)
        {
            this._dbConnection = DatabaseConnection;
        }

        private OrderItem dataReaderToObject(SqlDataReader dataReader)
        {
            return new OrderItem()
            {
                OrderThumbprint = dataReader["OrderThumbprint"].ToString(),
                Name = dataReader["ItemName"].ToString(),
                ItemPrice = dataReader["ItemPrice"].ToString().ToDecimal(),
                TotalPrice = dataReader["TotalPrice"].ToString().ToDecimal(),
                ProductId = dataReader["ProductId"].ToString().ToInt(),
                Quantity = dataReader["Quantity"].ToString().ToInt()
            };
        }
        
        public List<OrderItem> GetForOrder(string OrderThumbprint)
        {
            List<OrderItem> returnMe = new List<OrderItem>();

            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM OrderItems WHERE OrderThumbprint=@ORDERID"
                })
                {
                    sqlCommand.Parameters.AddWithValue("ORDERID", OrderThumbprint);
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

        public void Create(List<OrderItem> items)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                foreach (OrderItem item in items)
                {
                    using (SqlCommand sqlCommand = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = "INSERT INTO OrderItems(OrderThumbprint, ItemName, ItemPrice, TotalPrice, ProductId, Quantity) " +
                                                    "VALUES(@OTHUMB, @INAME, @IPRICE, @TPRICE, @PRODID, @QUAN)"
                    })
                    {
                        sqlCommand.Parameters.Clear();
                        sqlCommand.Parameters.AddWithValue("@OTHUMB", item.OrderThumbprint);
                        sqlCommand.Parameters.AddWithValue("@INAME", item.Name);
                        sqlCommand.Parameters.AddWithValue("@IPRICE", item.ItemPrice);
                        sqlCommand.Parameters.AddWithValue("@TPRICE", item.TotalPrice);
                        sqlCommand.Parameters.AddWithValue("@PRODID", item.ProductId);
                        sqlCommand.Parameters.AddWithValue("@QUAN", item.Quantity);
                        sqlCommand.Connection.Open();
                        sqlCommand.ExecuteNonQuery();
                        sqlCommand.Connection.Close();
                    }
                }
            }
        }
    }
}
