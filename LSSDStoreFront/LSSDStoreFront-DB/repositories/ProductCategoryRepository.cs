using LSSD.StoreFront.Lib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LSSD.StoreFront.DB.repositories
{
    public class ProductCategoryRepository : IRepository<ProductCategory>
    {
        private DatabaseContext _dbConnection;
        private readonly Dictionary<int, ProductCategory> _cache = new Dictionary<int, ProductCategory>();
        private ProductCategory _nullObj = new ProductCategory();

        public ProductCategoryRepository(DatabaseContext DatabaseConnectionString)
        {
            this._dbConnection = DatabaseConnectionString;

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
                    CommandText = "SELECT * FROM ProductCategories WHERE IsFlaggedForDeletion=0"
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
                        _cache.Clear();
                        while (dbDataReader.Read())
                        {
                            // Try to parse the row into an object to return.
                            ProductCategory obj = dataReaderToObject(dbDataReader);

                            // If something has gone horribly wrong, don't put a null object in the list of objects to
                            // return. Just skip it instead.
                            if (obj != null)
                            {
                                _cache.Add(obj.Id, obj);
                            }
                        }
                    }

                    // Close the database connection
                    sqlCommand.Connection.Close();
                } // Dispose of the SQL query object
            } // Dispose of the SQL connection object

            // Link categories together in parent/child relationships
            foreach(ProductCategory cat in _cache.Values)
            {
                if (cat.HasParent)
                {
                    if (_cache.ContainsKey(cat.ParentCategoryId))
                    {
                        cat.ParentCategory = _cache[cat.ParentCategoryId];
                        _cache[cat.ParentCategoryId].ChildCategories.Add(cat);
                    }
                }
            }
        }

        private ProductCategory dataReaderToObject(SqlDataReader dataReader)
        {
            return new ProductCategory()
            {
                Id = dataReader["Id"].ToString().ToInt(),
                Name = dataReader["Name"].ToString(),
                ParentCategoryId = dataReader["ParentCategoryId"].ToString().ToInt(),
                IsEnabled = dataReader["IsEnabled"].ToString().ToBool(),
                AvailableFromDate = dataReader["AvailableFrom"].ToString().ToDateTime(),
                AvailableToDate = dataReader["AvailableTo"].ToString().ToDateTime(),
            };
        }
        
        public void Create(ProductCategory obj)
        {
            throw new NotImplementedException();
        }

        public void Create(List<ProductCategory> obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(ProductCategory obj)
        {
            throw new NotImplementedException();
        }

        public ProductCategory Get(int id)
        {
            if (_cache.ContainsKey(id))
            {
                return _cache[id];
            }
            return _nullObj;
        }

        public List<ProductCategory> GetAll()
        {
            return _cache.Values.ToList<ProductCategory>(); 
        }

        public List<ProductCategory> GetAllTopLevel()
        {
            return _cache.Values.Where(x => x.IsTopLevel).ToList<ProductCategory>();
        }

        public void UnDelete(ProductCategory obj)
        {
            throw new NotImplementedException();
        }

        public void Update(ProductCategory obj)
        {
            throw new NotImplementedException();
        }
    }
}
