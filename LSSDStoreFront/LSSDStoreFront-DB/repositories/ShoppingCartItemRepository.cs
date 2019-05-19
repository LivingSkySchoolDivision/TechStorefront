using LSSD.StoreFront.DB.repositories;
using LSSD.StoreFront.Lib;
using LSSD.StoreFront.Lib.UserAccounts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LSSD.StoreFront.DB.repositories
{
    class ShoppingCartItemRepository
    {
        private readonly DatabaseContext _dbConnection;
        private readonly ProductRepository _productRepository;

        public ShoppingCartItemRepository(DatabaseContext DatabaseContext)
        {
            this._dbConnection = DatabaseContext;
            this._productRepository = new ProductRepository(this._dbConnection);
        }

        private ShoppingCartItem dataReaderToObject(SqlDataReader dataReader)
        {
            int productID = dataReader["ProductId"].ToString().ToInt();
            Product product = _productRepository.Get(productID);
            if (product == null) { return null; }

            return new ShoppingCartItem()
            {
                UserThumbprint = dataReader["UserThumbprint"].ToString(),
                ProductId = productID,
                Quantity = dataReader["Quantity"].ToString().ToInt(),
                Product = product
            };
        }

        public void Create(ShoppingCartItem obj)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                Create(obj, connection);
            }
        }

        public void Create(List<ShoppingCartItem> obj)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                foreach (ShoppingCartItem item in obj)
                {
                    Create(item, connection);
                }
            }
        }


        public void Create(List<ShoppingCartItem> obj, SqlConnection connection)
        {
            foreach(ShoppingCartItem item in obj)
            {
                Create(item, connection);
            }
        }


        private void Create(ShoppingCartItem obj, SqlConnection connection)
        {
            using (SqlCommand sqlCommand = new SqlCommand
            {
                Connection = connection,
                CommandType = CommandType.Text,
                CommandText = "INSERT INTO ShoppingCartItems(UserThumbprint, ProductId, Quantity) VALUES(@USERTHUMB, @PRODUCTID, @QUANTITY)"
            })
            {
                sqlCommand.Parameters.AddWithValue("USERTHUMB", obj.UserThumbprint);
                sqlCommand.Parameters.AddWithValue("PRODUCTID", obj.ProductId);
                sqlCommand.Parameters.AddWithValue("QUANTITY", obj.Quantity);
                sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Connection.Close();
            }
        }


        public List<ShoppingCartItem> GetAllForUser(UserThumbprint UserThumbprint)
        {
            List<ShoppingCartItem> returnMe = new List<ShoppingCartItem>();

            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM ShoppingCartItems WHERE UserThumbprint=@USERID"
                })
                {
                    sqlCommand.Parameters.AddWithValue("USERID", UserThumbprint.Value);
                    sqlCommand.Connection.Open();
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            ShoppingCartItem obj = dataReaderToObject(dbDataReader);
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
        
        public void ClearForUser(UserThumbprint userThumbprint)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                ClearForUser(userThumbprint, connection);
            }
        }

        public void ClearForUser(UserThumbprint userThumbprint, SqlConnection connection)
        {
            using (SqlCommand sqlCommand = new SqlCommand
            {
                Connection = connection,
                CommandType = CommandType.Text,
                CommandText = "DELETE FROM ShoppingCartItems WHERE UserThumbprint=@USERID;"
            })
            {
                sqlCommand.Parameters.AddWithValue("USERID", userThumbprint.Value);
                sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Connection.Close();
            }            
        }

        public void UpdateUserCartItems(UserThumbprint userThumbprint, List<ShoppingCartItem> items)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                this.ClearForUser(userThumbprint, connection);
                this.Create(items, connection);
            }
        }
    }
}
