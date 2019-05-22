using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LSSD.StoreFront.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace LSSD.StoreFront.FrontEnd.Pages
{
    public class ShoppingCartModel : PageModel
    {
        private readonly DatabaseContext dbContext;
        public UserFriendlyShoppingCart ShoppingCart;

        public void OnGet()
        {
            ShoppingCart = new UserFriendlyShoppingCart(dbContext, User.Identity.Name);
        }

        public ShoppingCartModel(IConfiguration config)
        {
            dbContext = new DatabaseContext(config.GetConnectionString(FrontendSettings.ConnectionStringName));
        }

        public IActionResult OnPostClearCart()
        {
            ShoppingCart = new UserFriendlyShoppingCart(dbContext, User.Identity.Name);
            ShoppingCart.ClearCart();
            ShoppingCart.Save();
            return RedirectToPage("/ShoppingCart");
        }

        public IActionResult OnPostUpdateQuantity()
        {
            ShoppingCart = new UserFriendlyShoppingCart(dbContext, User.Identity.Name);
            int productId = (Request.Form["productId"].ToString() ?? string.Empty).ToInt();
            int newQuantity = (Request.Form["quantity"].ToString() ?? string.Empty).ToInt();
            ShoppingCart.UpdateQuantity(productId, newQuantity);
            ShoppingCart.Save();
            return RedirectToPage("/ShoppingCart");
        }

        public IActionResult OnPostRemoveItem()
        {
            ShoppingCart = new UserFriendlyShoppingCart(dbContext, User.Identity.Name);
            int productId = (Request.Form["productId"].ToString() ?? string.Empty).ToInt();
            ShoppingCart.ClearItem(productId);
            ShoppingCart.Save();
            return RedirectToPage("/ShoppingCart");
        }

        public IActionResult OnPostCheckout()
        {
            return RedirectToPage("/Checkout");
        }
    }
}