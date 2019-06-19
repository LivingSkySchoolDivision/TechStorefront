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
                ItemBasePrice = dataReader["ItemPrice"].ToString().ToDecimal(),
                ItemGST = dataReader["ItemGST"].ToString().ToDecimal(),
                ItemPST = dataReader["ItemPST"].ToString().ToDecimal(),
                ItemEHF = dataReader["ItemEHF"].ToString().ToDecimal(),
                ItemPriceWithTax = dataReader["ItemPriceWithTax"].ToString().ToDecimal(),
                TotalBasePrice = dataReader["TotalPreTaxPrice"].ToString().ToDecimal(),
                TotalGST = dataReader["TotalGST"].ToString().ToDecimal(),
                TotalPST = dataReader["TotalPST"].ToString().ToDecimal(),
                TotalEHF = dataReader["TotalEHF"].ToString().ToDecimal(),
                TotalPriceWithTax = dataReader["TotalPrice"].ToString().ToDecimal(),
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
                        CommandText = "INSERT INTO OrderItems(OrderThumbprint, ItemName, ProductId, Quantity, TotalPrice, TotalPreTaxPrice, TotalGST, TotalPST, TotalEHF, ItemPrice, ItemGST, ItemPST, ItemEHF, ItemPriceWithTax) " +
                                                    "VALUES(@OTHUMB, @INAME, @PRODID, @QUAN, @TOTPRICE, @TOTPRETAXPRICE, @TOTGST, @TOTPST, @TOTEHF, @ITPRETAXPRICE, @ITGST, @ITPST, @ITEHF, @ITTOTALPRICE)"
                    })
                    {
                        sqlCommand.Parameters.Clear();
                        sqlCommand.Parameters.AddWithValue("@OTHUMB", item.OrderThumbprint);
                        sqlCommand.Parameters.AddWithValue("@INAME", item.Name);
                        sqlCommand.Parameters.AddWithValue("@PRODID", item.ProductId);
                        sqlCommand.Parameters.AddWithValue("@QUAN", item.Quantity);
                        sqlCommand.Parameters.AddWithValue("@TOTPRICE", item.TotalPriceWithTax);
                        sqlCommand.Parameters.AddWithValue("@TOTPRETAXPRICE", item.TotalBasePrice);
                        sqlCommand.Parameters.AddWithValue("@TOTGST", item.TotalGST);
                        sqlCommand.Parameters.AddWithValue("@TOTPST", item.TotalPST);
                        sqlCommand.Parameters.AddWithValue("@TOTEHF", item.TotalEHF);
                        sqlCommand.Parameters.AddWithValue("@ITPRETAXPRICE", item.ItemBasePrice);
                        sqlCommand.Parameters.AddWithValue("@ITGST", item.ItemGST);
                        sqlCommand.Parameters.AddWithValue("@ITPST", item.ItemPST);
                        sqlCommand.Parameters.AddWithValue("@ITEHF", item.ItemEHF);
                        sqlCommand.Parameters.AddWithValue("@ITTOTALPRICE", item.ItemPriceWithTax);
                        sqlCommand.Connection.Open();
                        sqlCommand.ExecuteNonQuery();
                        sqlCommand.Connection.Close();
                    }
                }
            }
        }
    }
}
