using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using LSSD.StoreFront.DB;
using LSSD.StoreFront.Lib;

namespace LSSD.StoreFront.FrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        public UserFriendlyInventory Inventory;
        
        public void OnGet()
        {     
        }


        public IndexModel(IConfiguration config)
        {
            // set up a database connection            
            DatabaseContext dbConnection = new DatabaseContext(config.GetConnectionString(FrontendSettings.ConnectionStringName));
            Inventory = new UserFriendlyInventory(dbConnection);

        }
    }
}
