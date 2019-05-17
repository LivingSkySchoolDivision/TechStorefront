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
        //public List<Product> AllProducts = new List<Product>();
        public Inventory Inventory;
        
        public void OnGet()
        {
            // set up a database connection
            string connectionString = @"data source=dc-sql2012.lskysd.ca\DCSQL2012;initial catalog=LSKYStorefront;user id=LSSDStoreFront-TEST;password=xyCTEq4tuXBEKPCRgID5dYWUxB8Yklwr;Trusted_Connection=false";
            DatabaseContext dbConnection = new DatabaseContext(connectionString);
            User User = new User()
            {
                Id = 0,
                Username = "Test User"
            };

            Inventory = new Inventory(dbConnection);







        }

        public IndexModel(IConfiguration config)
        {          
        }
    }
}
