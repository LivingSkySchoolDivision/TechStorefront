using LSSD.StoreFront.Lib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace LSSD.StoreFront.DB.repositories
{
    public class ProductRepository : IRepository<Product>
    {
        // Connection string to use when talking to the database.
        // Saved when passed from the constructor when a new repository object is created.
        // Retained for the lifetime of this object.
        private readonly DatabaseContext _dbConnection;
        private readonly ProductCategoryRepository _categoryRepository;

        // Base SQL query to use. Methods below may add more things to this (like " WHERE" clauses), or
        // they may use their own queries.
        private const string SQL_Select = "SELECT * FROM Products WHERE IsFlaggedForDeletion=0";

        // Constructor
        public ProductRepository(DatabaseContext DatabaseConnection)
        {
            this._dbConnection = DatabaseConnection;
            this._categoryRepository = new ProductCategoryRepository(this._dbConnection);
        }

        /// <summary>
        /// Convert a SqlDataReader object (a database row returned by a query) to a Product.
        /// </summary>
        /// <param name="dataReader">SqlDataReader object returned from the SQL Query</param>
        /// <returns>A shiny new Product object</returns>
        private Product dataReaderToObject(SqlDataReader dataReader)
        {
            // Create a new Product object, and insert parts of the returned SQL results.
            // SQL results are parsed to a string first, and then to their own data type
            // We can't directly parse them to C# data types because SQL data types and C# 
            // data types don't exactly match up.
            return new Product()
            {
                Id = dataReader["Id"].ToString().ToInt(),
                CategoryId = dataReader["CategoryId"].ToString().ToInt(),
                Name = dataReader["ProductName"].ToString(),
                Description = dataReader["ProductDescription"].ToString(),
                LongDescription = dataReader["ProductLongDescription"].ToString(),
                Alert = dataReader["ProductAlerts"].ToString(),
                ThumbnailFileName = dataReader["ThumbnailFileName"].ToString(),
                Price = dataReader["Price"].ToString().ToDecimal(),
                IsAvailable = dataReader["IsProductAvailable"].ToString().ToBool(),
                IsLimitedByStock = dataReader["IsLimitedByStock"].ToString().ToBool(),
                IsLimitedByDate = dataReader["IsLimitedByDate"].ToString().ToBool(),
                IsSpecialOrderItem = dataReader["IsSpecialOrderItem"].ToString().ToBool(),
                AvailableFromDate = dataReader["AvailableFrom"].ToString().ToDateTime(),
                AvailableToDate = dataReader["AvailableTo"].ToString().ToDateTime(),
                RecyclingFee = dataReader["RecyclingFee"].ToString().ToDecimal(),
                Category = _categoryRepository.Get(dataReader["CategoryId"].ToString().ToInt())
            };
        }

        /// <summary>
        /// Return ALL products in the database (including unavailable ones). Does not included Products flagged for deletion.
        /// </summary>
        /// <returns></returns>
        public List<Product> GetAll()
        {
            // Create an empty list to store the results (if any) in.
            List<Product> returnMe = new List<Product>();

            // Open database connection
            // The "using" blocks ensure that the objects get disposed of, so 
            // we (hopefully) don't leak memory without realizing it.
            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                // Build a SQL query
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = SQL_Select
                })
                {
                    // Open the database connection
                    sqlCommand.Connection.Open();

                    // Get the results of the query
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    // If the query returned any rows
                    if (dbDataReader.HasRows)
                    {
                        // Read all the results, one row at a time, until there are no more to  read
                        while (dbDataReader.Read())
                        {
                            // Try to parse the row into an object to return.
                            Product product = dataReaderToObject(dbDataReader);

                            // If something has gone horribly wrong, don't put a null object in the list of objects to
                            // return. Just skip it instead.
                            if (product != null)
                            {
                                returnMe.Add(product);
                            }
                        }
                    }

                    // Close the database connection
                    sqlCommand.Connection.Close();
                } // Dispose of the SQL query object
            } // Dispose of the SQL connection object

            // Return the list of objects to the user
            return returnMe;
        }

        /// <summary>
        /// Return all products in the specified category.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public List<Product> GetAllInCategory(ProductCategory category)
        {
            return GetAllInCategory(category.Id);
        }

        /// <summary>
        /// Return all products in the specified category (by ID number).
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public List<Product> GetAllInCategory(int categoryId)
        {
            List<Product> returnMe = new List<Product>();

            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = SQL_Select + " AND CategoryId=@CATID"
                })
                {
                    sqlCommand.Parameters.AddWithValue("CATID", categoryId);
                    sqlCommand.Connection.Open();
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            Product product = dataReaderToObject(dbDataReader);
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

        /// <summary>
        /// Get a specific Product, given it's Id number.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Product Get(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = SQL_Select + " AND Id=@OBJID"
                })
                {
                    sqlCommand.Parameters.AddWithValue("OBJID", Id);
                    sqlCommand.Connection.Open();
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            Product product = dataReaderToObject(dbDataReader);
                            if (product != null)
                            {
                                return product;
                            }
                        }
                    }

                    sqlCommand.Connection.Close();
                }
            }

            return null;
        }


        /// <summary>
        /// Update a Product object. You cannot update a product's Id number.
        /// </summary>
        /// <param name="product"></param>
        public void Update(Product product)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "UPDATE Products SET CategoryId=@CATID, ProductName=@NAME, ProductDescription=@DESC, ProductLongDescription=@LDESC, ProductAlerts=@ALERT, Price=@PRICE, IsProductAvailable=@ISAVAIL, IsLimitedByStock=@ISLIMITDBYSTOCK, IsLimitedByDate=@ISLIMITEDBYDATE, IsSpecialOrderItem=@ISSPECIALORDER, AvailableFrom=@AVAILFROM, AvailableTo=@AVAILTO, ThumbnailFileName=@THUMBFILE, RecyclingFee=@RECYCLINGFEE WHERE Id=@OBJID  "
                })
                {
                    sqlCommand.Parameters.AddWithValue("OBJID", product.Id);
                    sqlCommand.Parameters.AddWithValue("CATID", product.CategoryId);
                    sqlCommand.Parameters.AddWithValue("NAME", product.Name ?? string.Empty);
                    sqlCommand.Parameters.AddWithValue("DESC", product.Description ?? string.Empty);
                    sqlCommand.Parameters.AddWithValue("LDESC", product.LongDescription ?? string.Empty);
                    sqlCommand.Parameters.AddWithValue("ALERT", product.Alert ?? string.Empty);
                    sqlCommand.Parameters.AddWithValue("PRICE", product.Price);
                    sqlCommand.Parameters.AddWithValue("ISAVAIL", product.IsAvailable);
                    sqlCommand.Parameters.AddWithValue("ISLIMITDBYSTOCK", product.IsLimitedByStock);
                    sqlCommand.Parameters.AddWithValue("ISLIMITEDBYDATE", product.IsLimitedByDate);
                    sqlCommand.Parameters.AddWithValue("ISSPECIALORDER", product.IsSpecialOrderItem);
                    sqlCommand.Parameters.AddWithValue("AVAILFROM", product.AvailableFromDate.ToDatabaseSafeDateTime());
                    sqlCommand.Parameters.AddWithValue("AVAILTO", product.AvailableToDate.ToDatabaseSafeDateTime());
                    sqlCommand.Parameters.AddWithValue("THUMBFILE", product.ThumbnailFileName ?? string.Empty);
                    sqlCommand.Parameters.AddWithValue("RECYCLINGFEE", product.RecyclingFee);
                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
        }

        /// <summary>
        /// Delete a Product from the database.
        /// </summary>
        /// <param name="product"></param>
        public void Delete(Product product)
        {
            // Deleting a table row is fairly permanent, so just hide the object instead
            // by flagging it as deleted. Then, we can either restore it later, or
            // clean it up outside this program.

            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "UPDATE Products SET IsFlaggedForDeletion=1 WHERE Id=@OBJID"
                })
                {
                    sqlCommand.Parameters.AddWithValue("OBJID", product.Id);
                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
        }

        /// <summary>
        /// Undelete a product, if you know it's ID.
        /// </summary>
        /// <param name="product"></param>
        public void UnDelete(int productId)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "UPDATE Products SET IsFlaggedForDeletion=0 WHERE Id=@OBJID"
                })
                {
                    sqlCommand.Parameters.AddWithValue("OBJID", productId);
                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
        }

        /// <summary>
        /// Undelete a product
        /// </summary>
        /// <param name="product"></param>
        public void UnDelete(Product product)
        {
            UnDelete(product.Id);
        }

        /// <summary>
        /// Store a new product in the database
        /// </summary>
        /// <param name="product"></param>
        public void Create(Product product)
        {
            // Don't repeat ourselves, just put the object in a list and re-use the other Create method
            Create(new List<Product>() { product });
        }

        /// <summary>
        /// Store multiple new products in the database
        /// </summary>
        /// <param name="products"></param>
        public void Create(List<Product> products)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
            {
                foreach(Product product in products) {
                    using (SqlCommand sqlCommand = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = "INSERT INTO Products(CategoryId, ProductName, ProductDescription, ProductLongDescription, Price, IsProductAvailable, IsLimitedByStock, IsLimitedByDate, IsSpecialOrderItem, AvailableFrom, AvailableTo, ThumbnailFileName, ProductAlerts, RecyclingFee) " +
                                                    "VALUES(@CID, @NME, @DSC, @LDSC, @PRCE,  @LAVAIL, @LSTOCK, @LDATE, @LSCL, @DFRM, @DTO, @THMB, @ALRT, @RECYCLINGFEE)"
                    })
                    {
                        sqlCommand.Parameters.Clear();
                        sqlCommand.Parameters.AddWithValue("@CID", product.CategoryId);
                        sqlCommand.Parameters.AddWithValue("@NME", product.Name ?? string.Empty);
                        sqlCommand.Parameters.AddWithValue("@DSC", product.Description ?? string.Empty);
                        sqlCommand.Parameters.AddWithValue("@LDSC", product.Description ?? string.Empty);
                        sqlCommand.Parameters.AddWithValue("@PRCE", product.Price);
                        sqlCommand.Parameters.AddWithValue("@LAVAIL", product.IsAvailable);
                        sqlCommand.Parameters.AddWithValue("@LSTOCK", product.IsLimitedByStock);
                        sqlCommand.Parameters.AddWithValue("@LDATE", product.IsLimitedByDate);
                        sqlCommand.Parameters.AddWithValue("@LSCL", product.IsSpecialOrderItem);
                        sqlCommand.Parameters.AddWithValue("@DFRM", product.AvailableFromDate.ToDatabaseSafeDateTime());
                        sqlCommand.Parameters.AddWithValue("@DTO", product.AvailableToDate.ToDatabaseSafeDateTime());
                        sqlCommand.Parameters.AddWithValue("@THMB", product.ThumbnailFileName ?? string.Empty);
                        sqlCommand.Parameters.AddWithValue("@ALRT", product.Alert ?? string.Empty);
                        sqlCommand.Parameters.AddWithValue("@RECYCLINGFEE", product.RecyclingFee);
                        sqlCommand.Connection.Open();
                        sqlCommand.ExecuteNonQuery();
                        sqlCommand.Connection.Close();
                    }
                }
            }
        }
    }
}
