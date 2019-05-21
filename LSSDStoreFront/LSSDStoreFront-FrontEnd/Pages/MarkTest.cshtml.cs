using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LSSD.StoreFront.DB;
using LSSD.StoreFront.Lib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace LSSDStoreFront_FrontEnd.Pages
{
    public class MarkTestModel : PageModel
    {
        private readonly DatabaseContext dbContext;
        public UserFriendlyShoppingCart ShoppingCart;
        public UserFriendlyInventory Inventory;

        public void OnGet()
        {
            Inventory = new UserFriendlyInventory(dbContext);
            ShoppingCart = new UserFriendlyShoppingCart(dbContext, User.Identity.Name);
        }

        public MarkTestModel(IConfiguration config)
        {
            dbContext = new DatabaseContext(config.GetConnectionString("InternalDatabase"));
        }

        public IActionResult OnPostAddTestItems()
        {
            // This doesn't need to be cryptographically random
            Random random = new Random(DateTime.Now.Millisecond);

            UserFriendlyShoppingCart ShoppingCart = new UserFriendlyShoppingCart(dbContext, User.Identity.Name);

            List<int> testProductIds = new List<int>() { 2, 24, 25, 26, 3, 51, 47 };

            foreach (int productId in testProductIds)
            {
                ShoppingCart.AddItem(productId, random.Next(1,100));
            }

            ShoppingCart.Save();
            return RedirectToPage("/ShoppingCart");
        }
    }
}