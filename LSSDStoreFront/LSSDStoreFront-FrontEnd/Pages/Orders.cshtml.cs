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
    public class OrdersModel : PageModel
    {
        public OrdersModel(IConfiguration config)
        {
        }

        public void OnGet()
        {

        }
    }
}