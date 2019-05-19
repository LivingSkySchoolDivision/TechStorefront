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
    public class MarkTestModel : PageModel
    {
        private readonly DatabaseContext dbContext;
        public ShoppingCart ShoppingCart;
        public Inventory Inventory;

        public void OnGet()
        {
            Inventory = new Inventory(dbContext);
            ShoppingCart = new ShoppingCart(dbContext, User.Identity.Name);
        }

        public MarkTestModel(IConfiguration config)
        {
            dbContext = new DatabaseContext(config.GetConnectionString("InternalDatabase"));
        }
    }
}