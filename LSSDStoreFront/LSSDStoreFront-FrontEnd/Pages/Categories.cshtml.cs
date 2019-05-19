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
    public class CategoriesModel : PageModel
    {
        private DatabaseContext dbContext;
        public ShoppingCart ShoppingCart;
        public Inventory Inventory;

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public void OnGet()
        {
            
            ShoppingCart = new ShoppingCart(dbContext, User.Identity.Name);
        }

        public CategoriesModel(IConfiguration config)
        {
            dbContext = new DatabaseContext(config.GetConnectionString("InternalDatabase"));
            Inventory = new Inventory(dbContext);
        }
    }
}