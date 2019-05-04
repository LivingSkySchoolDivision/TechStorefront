using LSSD.StoreFront.Lib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LSSD.StoreFront.DB
{
    public class ProductRepository
    {
        private string dbConnectionString = string.Empty;
        private Dictionary<int, Product> _allProducts = new Dictionary<int, Product>()
        {
            {
                0, new Product()
                {
                    Id = 0,
                    CategoryId = 0,
                    Name = "Test Product 1",
                    Description = "A test product, which doesn't actually exist",
                    Price = (decimal)1.99,
                    IsAvailable = true,
                    IsLimitedByDate = false,
                    IsLimitedByStock = false,
                    IsSpecialOrderItem = false
                }
            },
            {
                1, new Product()
                {
                    Id = 0,
                    CategoryId = 0,
                    Name = "Test Product 2",
                    Description = "A test product, which doesn't actually exist",
                    Price = (decimal)1.99,
                    IsAvailable = true,
                    IsLimitedByDate = false,
                    IsLimitedByStock = false,
                    IsSpecialOrderItem = false
                }
            },
            {
                2, new Product()
                {
                    Id = 0,
                    CategoryId = 0,
                    Name = "Test Product 3",
                    Description = "A test product, which doesn't actually exist",
                    Price = (decimal)1.99,
                    IsAvailable = true,
                    IsLimitedByDate = false,
                    IsLimitedByStock = false,
                    IsSpecialOrderItem = false
                }
            }

        };

        public ProductRepository(string DatabaseConnectionString)
        {
            this.dbConnectionString = DatabaseConnectionString;
        }

        public List<Product> GetAll()
        {
            return _allProducts.Values.ToList();
        }

        public Product Get(int id)
        {
            if (_allProducts.ContainsKey(id))
            {
                return _allProducts[id];
            } else
            {
                return null;
            }
        }

        public void Update(Product product)
        {
            throw new NotImplementedException();
        }

        public void Delete(Product product)
        {
            throw new NotImplementedException();
        }

        public void Create(Product product)
        {
            throw new NotImplementedException();
        }

        public void Create(List<Product> products)
        {
            throw new NotImplementedException();
        }
    }
}
