using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using LSSD.StoreFront.DB;
using LSSD.StoreFront.Lib;

namespace LSSDStoreFront_FrontEnd.Pages
{
    public class AllProductsModel : PageModel
    {
        public UserFriendlyInventory Inventory;

        public void OnGet()
        {
        }

        public AllProductsModel(IConfiguration config)
        {
            // set up a database connection            
            DatabaseContext dbConnection = new DatabaseContext(config.GetConnectionString("InternalDatabase"));
            Inventory = new UserFriendlyInventory(dbConnection);

        }
    }
}