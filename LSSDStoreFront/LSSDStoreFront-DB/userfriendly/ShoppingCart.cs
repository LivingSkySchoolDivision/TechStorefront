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
        private Dictionary<int, ShoppingCartItem> _items { get; set; }

        private DatabaseContext _dbContext;
        private User _user;
        private ShoppingCartItemRepository _shoppingCartItemRepository;
        private ProductRepository _productRepository;
        
                
        public bool HasItems
        {
            get
            {
                return this._items.Count > 0;
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
                this.Add(sci);
            }
        }


        public Order CheckOut()
        {
            // Convert this shopping cart to an order
            return new Order();
        }


        public void Save()
        {
            // Save this shopping cart to the database

            // If the list is empty, delete any shopping carts for this user
            // If a shopping cart exists for this user already, delete it and remake it

            // DELETE SHOPPING CARTS FOR THIS USER
            // here

            if (this.HasItems)
            {
                // CREATE NEW SHOPPING CART FOR THIS USER
                // here
            }

        }

        // ********************************************************
        // * Add Items
        // ********************************************************

        public void Add(int ProductId, int Quantity)
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

        public void Add(Product product, int Quantity)
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

        private void Add(ShoppingCartItem item)
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

        public void Remove(int ProductId, int Quantity)
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

        public void Remove(Product product, int Quantity)
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

        public void Remove(ShoppingCartItem item)
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
        // * Set Quantity of Items
        // ********************************************************

        public void SetQuantity(int ProductId, int Quantity)
        {
            if (ProductId > 0)
            {
                if (_items.ContainsKey(ProductId))
                {
                    if (Quantity <= 0)
                    {
                        ClearProduct(ProductId);
                    } else
                    {
                        _items[ProductId].Quantity = Quantity;
                    }
                } else
                {
                    if (Quantity > 0)
                    {
                        Add(ProductId, Quantity);
                    }
                }
            }
        }

        public void SetQuantity(Product product, int Quantity)
        {
            if (product != null)
            {                
                if (_items.ContainsKey(product.Id))
                {
                    if (Quantity <= 0)
                    {
                        ClearProduct(product);
                    }
                    else
                    {
                        _items[product.Id].Quantity = Quantity;
                    }
                }
                else
                {
                    if (Quantity > 0)
                    {
                        Add(product, Quantity);
                    }
                }
                
            }
        }

        public void SetQuantity(ShoppingCartItem ShoppingCartItem)
        {
            Update(ShoppingCartItem);
        }

        public void Update(ShoppingCartItem ShoppingCartItem)
        {
            if (ShoppingCartItem != null)
            {
                if (_items.ContainsKey(ShoppingCartItem.ProductId))
                {
                    if (ShoppingCartItem.Quantity <= 0)
                    {
                        ClearProduct(ShoppingCartItem);
                    }
                    else
                    {
                        _items[ShoppingCartItem.ProductId] = ShoppingCartItem;
                    }
                }
                else
                {
                    if (ShoppingCartItem.Quantity > 0)
                    {
                        Add(ShoppingCartItem);
                    }
                }
            }
        }
               
       
        // ********************************************************
        // * Clear Items
        // ********************************************************

        public void ClearProduct(Product product)
        {
            if (product != null)
            {
                if (_items.ContainsKey(product.Id))
                {
                    _items.Remove(product.Id);
                }
            }
        }

        public void ClearProduct(ShoppingCartItem item)
        {
            if (item != null)
            {
                if (_items.ContainsKey(item.ProductId))
                {
                    _items.Remove(item.ProductId);
                }
            }
        }

        public void ClearProduct(int ProductId)
        {
            if (_items.ContainsKey(ProductId))
            {
                _items.Remove(ProductId);
            }
        }
                
        public void Clear()
        {
            _items.Clear();
        }

    }
}
