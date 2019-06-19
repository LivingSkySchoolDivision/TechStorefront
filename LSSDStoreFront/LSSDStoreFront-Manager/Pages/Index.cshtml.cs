using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LSSD.StoreFront.Lib.Email;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using LSSD.StoreFront.DB;

namespace LSSD.StoreFront.Manager.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel(IConfiguration config)
        {
        }

        public void OnGet()
        {

        }
    }
}
