using LSSD.StoreFront.DB.repositories;
using LSSD.StoreFront.Lib;
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
                UserId = dataReader["UserId"].ToString().ToInt(),
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
                CommandText = "INSERT INTO ShoppingCartItems(UserId, ProductId, Quantity) VALUES(@USERID, @PRODUCTID, @QUANTITY)"
            })
            {
                sqlCommand.Parameters.AddWithValue("USERID", obj.UserId);
                sqlCommand.Parameters.AddWithValue("PRODUCTID", obj.ProductId);
                sqlCommand.Parameters.AddWithValue("QUANTITY", obj.Quantity);
                sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Connection.Close();
            }
        }


        public List<ShoppingCartItem> GetAllForUser(int UserID)
        {
            List<ShoppingCartItem> returnMe = new List<ShoppingCartItem>();

            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM ShoppingCartItems WHERE UserId=@USERID"
                })
                {
                    sqlCommand.Parameters.AddWithValue("USERID", UserID);
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

        public List<ShoppingCartItem> GetAllForUser(User user)
        {
            if (user == null)
            {
                return new List<ShoppingCartItem>();
            }

            return GetAllForUser(user.Id);
        }

        public void ClearForUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                ClearForUser(user, connection);
            }
        }

        public void ClearForUser(User user, SqlConnection connection)
        {
            using (SqlCommand sqlCommand = new SqlCommand
            {
                Connection = connection,
                CommandType = CommandType.Text,
                CommandText = "DELETE FROM ShoppingCartItems WHERE UserId=@USERID;"
            })
            {
                sqlCommand.Parameters.AddWithValue("USERID", user.Id);
                sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Connection.Close();
            }            
        }

        public void UpdateUserCartItems(User user, List<ShoppingCartItem> items)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                this.ClearForUser(user, connection);
                this.Create(items, connection);
            }
        }
    }
}
