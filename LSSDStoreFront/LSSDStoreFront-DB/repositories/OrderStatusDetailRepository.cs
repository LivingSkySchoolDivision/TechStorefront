using LSSD.StoreFront.Lib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace LSSD.StoreFront.DB.repositories
{
    class OrderStatusDetailRepository
    {
        private readonly DatabaseContext _dbConnection;
        public OrderStatusDetailRepository(DatabaseContext Database)
        {
            this._dbConnection = Database;
        }

        private OrderStatusDetail dataReaderToObject(SqlDataReader dataReader)
        {
            return new OrderStatusDetail()
            {
                Id = dataReader["Id"].ToString().ToInt(),
                OrderThumbprint = dataReader["OrderThumbprint"].ToString(),
                Status = dataReader["OrderStatus"].ToString(),
                Timestamp = dataReader["DetailTimeStamp"].ToString().ToDateTime(),
                Notes = dataReader["DetailNotes"].ToString(),
                UpdatedBy = dataReader["UpdatedBy"].ToString(),
            };
        }

        public List<OrderStatusDetail> GetForOrder(string OrderThumbprint)
        {
            List<OrderStatusDetail> returnMe = new List<OrderStatusDetail>();

            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM OrderStatusDetails WHERE OrderThumbprint=@ORDERID"
                })
                {
                    sqlCommand.Parameters.AddWithValue("ORDERID", OrderThumbprint);
                    sqlCommand.Connection.Open();

                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            OrderStatusDetail product = dataReaderToObject(dbDataReader);

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

        public void Create(List<OrderStatusDetail> items)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                foreach (OrderStatusDetail item in items)
                {
                    using (SqlCommand sqlCommand = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = "INSERT INTO OrderStatusDetails(OrderThumbprint, OrderStatus, DetailTimeStamp, DetailNotes, UpdatedBy) " +
                                                    "VALUES(@OTHUMB, @OSTATUS, @DTIMESTAMP, @DNOTES, @UPDATEDBY)"
                    })
                    {
                        sqlCommand.Parameters.Clear();
                        sqlCommand.Parameters.AddWithValue("@OTHUMB", item.OrderThumbprint);
                        sqlCommand.Parameters.AddWithValue("@OSTATUS", item.Status);
                        sqlCommand.Parameters.AddWithValue("@DTIMESTAMP", item.Timestamp);
                        sqlCommand.Parameters.AddWithValue("@DNOTES", item.Notes);
                        sqlCommand.Parameters.AddWithValue("@UPDATEDBY", item.UpdatedBy);
                        sqlCommand.Connection.Open();
                        sqlCommand.ExecuteNonQuery();
                        sqlCommand.Connection.Close();
                    }
                }
            }
        }
    }
}
