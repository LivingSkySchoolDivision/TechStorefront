using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LSSD.StoreFront.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace LSSDStoreFront_FrontEnd.Pages
{
    public class ShoppingCartModel : PageModel
    {
        private readonly DatabaseContext dbContext;
        public ShoppingCart ShoppingCart;

        public void OnGet()
        {
            ShoppingCart = new ShoppingCart(dbContext, User.Identity.Name);
        }

        public ShoppingCartModel(IConfiguration config)
        {
            dbContext = new DatabaseContext(config.GetConnectionString("InternalDatabase"));
        }

        public IActionResult OnPostClearCart()
        {
            ShoppingCart = new ShoppingCart(dbContext, User.Identity.Name);
            ShoppingCart.ClearCart();
            ShoppingCart.Save();
            return RedirectToPage("/ShoppingCart");
        }

        public IActionResult OnPostUpdateQuantity()
        {
            ShoppingCart = new ShoppingCart(dbContext, User.Identity.Name);
            int productId = (Request.Form["productId"].ToString() ?? string.Empty).ToInt();
            int newQuantity = (Request.Form["quantity"].ToString() ?? string.Empty).ToInt();
            ShoppingCart.UpdateQuantity(productId, newQuantity);
            ShoppingCart.Save();
            return RedirectToPage("/ShoppingCart");
        }

        public IActionResult OnPostRemoveItem()
        {
            ShoppingCart = new ShoppingCart(dbContext, User.Identity.Name);
            int productId = (Request.Form["productId"].ToString() ?? string.Empty).ToInt();
            ShoppingCart.ClearItem(productId);
            ShoppingCart.Save();
            return RedirectToPage("/ShoppingCart");
        }
    }
}