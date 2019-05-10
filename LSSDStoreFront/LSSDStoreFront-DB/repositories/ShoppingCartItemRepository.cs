using LSSD.StoreFront.DB.repositories;
using LSSD.StoreFront.Lib;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.DB.repositories
{
    class ShoppingCartItemRepository : IRepository<ShoppingCartItem>
    {
        private DatabaseContext _dbContext;
        private ProductRepository _productRepository;

        public ShoppingCartItemRepository(DatabaseContext DatabaseContext)
        {
            this._dbContext = DatabaseContext;
            this._productRepository = new ProductRepository(DatabaseContext);
        }

        public void Create(ShoppingCartItem obj)
        {
            throw new NotImplementedException();
        }

        public void Create(List<ShoppingCartItem> obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(ShoppingCartItem obj)
        {
            throw new NotImplementedException();
        }

        public ShoppingCartItem Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<ShoppingCartItem> GetAll()
        {
            throw new NotImplementedException();
        }

        public void UnDelete(ShoppingCartItem obj)
        {
            throw new NotImplementedException();
        }

        public void Update(ShoppingCartItem obj)
        {
            throw new NotImplementedException();
        }

        public List<ShoppingCartItem> GetAllForUser(int UserID)
        {
            return new List<ShoppingCartItem>();
        }

        public List<ShoppingCartItem> GetAllForUser(User user)
        {
            if (user == null)
            {
                return new List<ShoppingCartItem>();
            }

            return GetAllForUser(user.Id);
        }
    }
}
