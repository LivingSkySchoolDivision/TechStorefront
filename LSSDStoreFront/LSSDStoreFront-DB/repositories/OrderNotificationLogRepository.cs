using LSSD.StoreFront.Lib.Orders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace LSSD.StoreFront.DB.repositories
{
    public class OrderNotificationLogRepository
    {
        private readonly DatabaseContext _dbConnection;
        public OrderNotificationLogRepository(DatabaseContext DatabaseConnection)
        {
            this._dbConnection = DatabaseConnection;
        }

        private OrderNotificationLogEntry dataReaderToObject(SqlDataReader dataReader)
        {
            return new OrderNotificationLogEntry()
            {
                OrderThumbPrint = dataReader["OrderThumbprint"].ToString(),
                NotificationDate = dataReader["NotificationDate"].ToString().ToDateTime(),
                Recipient = dataReader["Recipient"].ToString(),
                WasSentWithoutError = dataReader["WasSentWithoutError"].ToString().ToBool(),
                WasToCustomer = dataReader["ToCustomer"].ToString().ToBool(),
                WasToOrderDesk = dataReader["ToOrderDesk"].ToString().ToBool(),
                Notes = dataReader["Notes"].ToString()
            };
        }


        public void LogNotification(string orderThumbprint, string Recipient, bool WasSuccess, bool WasToCustomer, bool WasToOrderDesk, string notes)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "INSERT INTO OrderNotifications(OrderThumbprint, NotificationDate, Recipient, WasSentWithoutError, ToCustomer, ToOrderDesk, Notes) VALUES(@OTHUMB,@NOTIDATE,@RECIP,@NOERROR,@TOCUST,@TOORDERD,@NOTES);"
                })
                {
                    sqlCommand.Parameters.AddWithValue("OTHUMB", orderThumbprint);
                    sqlCommand.Parameters.AddWithValue("NOTIDATE", DateTime.Now);
                    sqlCommand.Parameters.AddWithValue("RECIP", Recipient);
                    sqlCommand.Parameters.AddWithValue("NOERROR", WasSuccess);
                    sqlCommand.Parameters.AddWithValue("TOCUST", WasToCustomer);
                    sqlCommand.Parameters.AddWithValue("TOORDERD", WasToOrderDesk);
                    sqlCommand.Parameters.AddWithValue("NOTES", notes);

                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
        }

        public List<OrderNotificationLogEntry> GetForOrder(string OrderThumbprint)
        {
            List<OrderNotificationLogEntry> returnMe = new List<OrderNotificationLogEntry>();

            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM OrderNotifications WHERE OrderThumbprint=@ORDERID"
                })
                {
                    sqlCommand.Parameters.AddWithValue("ORDERID", OrderThumbprint);
                    sqlCommand.Connection.Open();

                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            OrderNotificationLogEntry notification = dataReaderToObject(dbDataReader);

                            if (notification != null)
                            {
                                returnMe.Add(notification);
                            }
                        }
                    }

                    sqlCommand.Connection.Close();
                }
            }

            return returnMe;
        }

        public List<string> GetOrdersNeedingCustomerNotifications()
        {
            List<string> returnMe = new List<string>();

            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT Orders.OrderThumbprint FROM Orders WHERE (SELECT COUNT(*) FROM OrderNotifications WHERE OrderThumbprint=Orders.OrderThumbprint AND WasSentWithoutError=1 AND ToCustomer=1) < 1;"
                })
                {
                    sqlCommand.Connection.Open();

                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            returnMe.Add(dbDataReader["OrderThumbprint"].ToString());
                        }
                    }

                    sqlCommand.Connection.Close();
                }
            }

            return returnMe;

        }

        public List<string> GetOrdersNeedingOrderDeskNotifications()
        {
            List<string> returnMe = new List<string>();

            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT Orders.OrderThumbprint FROM Orders WHERE (SELECT COUNT(*) FROM OrderNotifications WHERE OrderThumbprint=Orders.OrderThumbprint AND WasSentWithoutError=1 AND ToOrderDesk=1) < 1;"
                })
                {
                    sqlCommand.Connection.Open();

                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            returnMe.Add(dbDataReader["OrderThumbprint"].ToString());
                        }
                    }

                    sqlCommand.Connection.Close();
                }
            }

            return returnMe;

        }

        public List<OrderNotificationLogEntry> GetRecent(int count)
        {

            List<OrderNotificationLogEntry> returnMe = new List<OrderNotificationLogEntry>();

            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT TOP " +  count + " * FROM OrderNotifications ORDER BY NotificationDate DESC"
                })
                {
                    sqlCommand.Connection.Open();

                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            OrderNotificationLogEntry notification = dataReaderToObject(dbDataReader);

                            if (notification != null)
                            {
                                returnMe.Add(notification);
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
