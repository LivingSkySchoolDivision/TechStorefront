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
                OrderID = dataReader["OrderId"].ToString().ToInt(),
                Status = dataReader["OrderStatus"].ToString(),
                Timestamp = dataReader["DetailTimeStamp"].ToString().ToDateTime(),
                Notes = dataReader["DetailNotes"].ToString(),
                UpdatedBy = dataReader["UpdatedBy"].ToString(),
            };
        }

        public List<OrderStatusDetail> GetForOrder(int OrderId)
        {
            List<OrderStatusDetail> returnMe = new List<OrderStatusDetail>();

            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM OrderStatusDetails WHERE OrderId=@ORDERID"
                })
                {
                    sqlCommand.Parameters.AddWithValue("ORDERID", OrderId);
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
    }
}
