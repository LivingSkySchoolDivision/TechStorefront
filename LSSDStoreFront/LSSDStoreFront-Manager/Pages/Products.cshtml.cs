using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LSSD.StoreFront.DB;
using LSSD.StoreFront.DB.repositories;
using LSSD.StoreFront.Lib;
using LSSD.StoreFront.Lib.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace LSSD.StoreFront.Manager.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly DatabaseContext dbContext;

        public ProductsModel(IConfiguration config)
        {
            dbContext = new DatabaseContext(config.GetConnectionString(ManagerSettings.ConnectionStringName));
        }

        public void OnPostAddItem()
        {
            // Create a new item with a random name
            // Find the new item
            // Go to the edit page for this item
            ProductRepository productRepo = new ProductRepository(dbContext);
            string UserDisplayName = ((System.Security.Claims.ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == "name").FirstOrDefault().Value;
            string tempName = Crypto.Hash(DateTime.Now + "-" + UserDisplayName);
            Product newProduct = new Product()
            {
                Name = tempName
            };
            


        }

        public void OnGet()
        {

        }
    }
}