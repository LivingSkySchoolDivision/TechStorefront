using LSSD.StoreFront.DB.repositories;
using LSSD.StoreFront.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSSD.StoreFront.DB
{
    public class ShoppingCart
    {     
        public IReadOnlyList<ShoppingCartItem> Items
        {
            get
            {
                return this._items.Values.ToList();
            }
        }

        // <product ID, item object>
        private readonly Dictionary<int, ShoppingCartItem> _items = new Dictionary<int, ShoppingCartItem>();

        private readonly DatabaseContext _dbContext;
        private readonly User _user;
        private readonly ShoppingCartItemRepository _shoppingCartItemRepository;
        private readonly ProductRepository _productRepository;        
                
        public bool IsEmpty
        {
            get
            {
                return this._items.Count == 0;
            }
        }
        
        public ShoppingCart(DatabaseContext DatabaseContext, User UserAccount)
        {
            this._dbContext = DatabaseContext;
            this._user = UserAccount;

            this._shoppingCartItemRepository = new ShoppingCartItemRepository(this._dbContext);
            this._productRepository = new ProductRepository(this._dbContext);

            this._items = new Dictionary<int, ShoppingCartItem>();

            // Load this user's shopping cart
            foreach (ShoppingCartItem sci in _shoppingCartItemRepository.GetAllForUser(UserAccount))
            {
                this.AddItem(sci);
            }
        }

        public void Save()
        {
            _shoppingCartItemRepository.UpdateUserCartItems(_user, this._items.Values.ToList<ShoppingCartItem>());
        }

        // ********************************************************
        // * Add Items
        // ********************************************************

        public void AddItem(int ProductId, int Quantity)
        {
            if (Quantity != 0)
            {
                if (_items.ContainsKey(ProductId))
                {
                    // If the item is already in the cart, just modify it's quantity
                    _items[ProductId].Quantity += Quantity;

                    // Remove zero or negative quantity items
                    if (_items[ProductId].Quantity <= 0)
                    {
                        _items.Remove(ProductId);
                    }
                }
                else
                {
                    // If the item isn't in the cart, add it, but it must be a valid product first
                    if (Quantity > 0)
                    {
                        Product p = _productRepository.Get(ProductId);
                        if (p != null)
                        {
                            _items.Add(p.Id, p.ToShoppingCartItem(this._user, Quantity));
                        }
                    }
                }
            }
        }

        public void AddItem(Product product, int Quantity)
        {
            if (Quantity != 0)
            {
                if (product != null)
                {
                    if (!_items.ContainsKey(product.Id))
                    {
                        _items.Add(product.Id, product.ToShoppingCartItem(this._user, 0));
                    }

                    _items[product.Id].Quantity += Quantity;

                    if (_items[product.Id].Quantity <= 0)
                    {
                        _items.Remove(product.Id);
                    }

                    // Remove zero or negative quantity items
                    if (_items[product.Id].Quantity <= 0)
                    {
                        _items.Remove(product.Id);
                    }
                }
            }
        }

        private void AddItem(ShoppingCartItem item)
        {
            if (item != null)
            {
                if (item.Quantity != 0)
                {
                    if (item.ProductId > 0)
                    {
                        if (!_items.ContainsKey(item.ProductId))
                        {
                            // Load the Product object if it's not already loaded
                            if (item.Product == null)
                            {
                                Product p = _productRepository.Get(item.ProductId);
                                if (p == null)
                                {
                                    item.Product = p;
                                }
                                else
                                {
                                    return;
                                }
                            }

                            _items.Add(item.ProductId, item);
                        }
                        else
                        {
                            // If the cart already contains this item, merge them
                            _items[item.ProductId].Quantity += item.Quantity;
                        }

                        // Remove zero or negative quantity items
                        if (_items[item.ProductId].Quantity <= 0)
                        {
                            _items.Remove(item.ProductId);
                        }
                    }
                }
            }
        }


        // ********************************************************
        // * Remove Items
        // ********************************************************

        public void RemoveItem(int ProductId, int Quantity)
        {
            if (_items.ContainsKey(ProductId))
            {
                _items[ProductId].Quantity -= Quantity;

                // Remove zero or negative quantity items
                if (_items[ProductId].Quantity <= 0)
                {
                    _items.Remove(ProductId);
                }
            }
        }

        public void RemoveItem(Product product, int Quantity)
        {
            if (product != null)
            {
                if (_items.ContainsKey(product.Id))
                {
                    _items[product.Id].Quantity -= Quantity;

                    // Remove zero or negative quantity items
                    if (_items[product.Id].Quantity <= 0)
                    {
                        _items.Remove(product.Id);
                    }
                }
            }
        }

        public void RemoveItem(ShoppingCartItem item)
        {
            if (item != null)
            {
                if (_items.ContainsKey(item.ProductId))
                {
                    _items.Remove(item.ProductId);
                }
            }
        }

               
       
        // ********************************************************
        // * Clear Items
        // ********************************************************

        public void ClearItem(Product product)
        {
            if (product != null)
            {
                if (_items.ContainsKey(product.Id))
                {
                    _items.Remove(product.Id);
                }
            }
        }

        public void ClearItem(ShoppingCartItem item)
        {
            if (item != null)
            {
                if (_items.ContainsKey(item.ProductId))
                {
                    _items.Remove(item.ProductId);
                }
            }
        }

        public void ClearItem(int ProductId)
        {
            if (_items.ContainsKey(ProductId))
            {
                _items.Remove(ProductId);
            }
        }
                
        public void ClearCart()
        {
            _items.Clear();
        }

    }
}
