using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LSSD.StoreFront.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace LSSDStoreFront_FrontEnd.Pages
{
    public class ItemsModel : PageModel
    {
        private DatabaseContext dbContext;
        public UserFriendlyShoppingCart ShoppingCart;
        public UserFriendlyInventory Inventory;

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public void OnGet()
        {
            Inventory = new UserFriendlyInventory(dbContext);
            ShoppingCart = new UserFriendlyShoppingCart(dbContext, User.Identity.Name);



        }

        public ItemsModel(IConfiguration config)
        {
            dbContext = new DatabaseContext(config.GetConnectionString("InternalDatabase"));
        }
                
    }

}