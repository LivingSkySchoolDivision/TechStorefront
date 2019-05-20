using LSSD.StoreFront.DB.repositories;
using LSSD.StoreFront.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSSD.StoreFront.DB
{
    public class UserFriendlyInventory
    {
        public List<ProductCategory> AllCategories {
            get
            {
                return this._allCategories.Values.Where(x => x.IsEnabled).OrderBy(x => x.Name).ToList();
            }
        }

        public List<ProductCategory> TopLevelCategories
        {
            get
            {
                return this._allCategories.Values.Where(x => x.IsTopLevel).Where(x => x.IsEnabled).OrderBy(x => x.Name).ToList();
            }
        }

        public List<Product> Items {
            get
            {
                return this._allItems.Values.ToList();
            }
        }
               
        private readonly DatabaseContext _dbContext;
        private readonly Dictionary<int, Product> _allItems = new Dictionary<int, Product>();
        private readonly Dictionary<int, ProductCategory> _allCategories = new Dictionary<int, ProductCategory>(); 

        private readonly Product _nullProduct = new Product();
        private readonly ProductCategory _nullProductCategory = new ProductCategory();
        
        public UserFriendlyInventory(DatabaseContext DatabaseContext)
        {
            this._dbContext = DatabaseContext;      

            // Load all product categories into cache
            ProductCategoryRepository _productCategoryRepository = new ProductCategoryRepository(DatabaseContext);
            _allCategories.Clear();
            foreach(ProductCategory category in _productCategoryRepository.GetAll()) {
                _allCategories.Add(category.Id, category);
            }

            // Load all products into cache
            // This may have to be changed if we get a lot of products
            ProductRepository _productRepository = new ProductRepository(DatabaseContext);
            _allItems.Clear();
            foreach(Product product in _productRepository.GetAll()) {
                _allItems.Add(product.Id, product);
            }

        }
               
        public ProductCategory Category(int CategoryID)
        {
            if (_allCategories.ContainsKey(CategoryID)) {
                return _allCategories[CategoryID];
            } else {
                return _nullProductCategory;
            }
        }

        public ProductCategory Category(string CategoryName) {
            return _allCategories.Values.Where(x => x.Name.ToLower() == CategoryName.ToLower().Trim())?.FirstOrDefault() ?? _nullProductCategory;
        }

        public Product Item(int ItemID)
        {
            if (_allItems.ContainsKey(ItemID)) {
                return _allItems[ItemID];
            } else {
                return _nullProduct;
            }
        }

        public List<Product> ItemsFromCategory(ProductCategory Category)
        {
            return ItemsFromCategory(Category.Id);
        }

        public List<Product> ItemsFromCategory(int CategoryId)
        {
            if (CategoryId == 0)
            {
                return this.Items;
            }
            else
            {
                return _allItems.Values.Where(x => x.CategoryId == CategoryId).OrderBy(x => x.Name).ToList();
            }
        }





    }
}
