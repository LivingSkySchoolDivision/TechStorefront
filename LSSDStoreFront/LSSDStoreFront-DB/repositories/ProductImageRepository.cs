using LSSD.StoreFront.Lib.Products;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace LSSD.StoreFront.DB.repositories
{
    public class ProductImageRepository
    {
        private readonly DatabaseContext _dbConnection;

        private readonly List<int> _productsWithImages = new List<int>();

        public ProductImageRepository(DatabaseContext DatabaseConnection)
        {
            this._dbConnection = DatabaseConnection;

            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT productId FROM ProductImages"
                })
                {
                    sqlCommand.Connection.Open();

                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            int productId = dbDataReader["productId"].ToString().ToInt();
                            if (!_productsWithImages.Contains(productId))
                            {
                                _productsWithImages.Add(productId);
                            }
                        }
                    }

                    sqlCommand.Connection.Close();
                }
            }
        }

        public bool DoesProductHaveImage(int productId)
        {
            return _productsWithImages.Contains(productId);
        }

        private ProductImage dataReaderToObject(SqlDataReader dataReader)
        {
            return new ProductImage()
            {
                Id = dataReader["imageId"].ToString().ToInt(),
                ProductId = dataReader["productId"].ToString().ToInt(),
                ImageBytes = (byte[])dataReader["imageData"],
                ContentType = dataReader["imageType"].ToString()
            };
        }

        public ProductImage GetImageForProduct(int ProductId)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM ProductImages WHERE productId=@PRODID"
                })
                {
                    sqlCommand.Parameters.AddWithValue("PRODID", ProductId);
                    sqlCommand.Connection.Open();

                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            ProductImage img = dataReaderToObject(dbDataReader);

                            if (img != null)
                            {
                                return img;
                            }
                        }
                    }

                    sqlCommand.Connection.Close();
                }
            }

            return null;
        }

        public void Create(ProductImage image)
        {
            Create(image.ProductId, image.ContentType, image.ImageBytes);
        }

        public void Create(int productID, string contentType, byte[] imageData)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "DELETE FROM ProductImages WHERE productId=@PRODID; INSERT INTO ProductImages(productID, imageData, imageType) VALUES(@PRODID, @IMGDATA, @IMGTYPE)"
                })
                {
                    sqlCommand.Parameters.AddWithValue("PRODID", productID);
                    sqlCommand.Parameters.AddWithValue("IMGDATA", imageData);
                    sqlCommand.Parameters.AddWithValue("IMGTYPE", contentType);
                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
        }

    }
}
