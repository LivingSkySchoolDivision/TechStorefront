using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LSSD.StoreFront.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace LSSDStoreFront_Manager.Pages
{
    public class EmailModel : PageModel
    {
        public EmailModel(IConfiguration config)
        {
          }

        public void OnGet()
        {

        }

    }
}