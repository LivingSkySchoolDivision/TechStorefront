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
        EmailHelper email;

        public IndexModel(IConfiguration config)
        {
            IConfigurationSection emailSettings = config.GetSection("SMTP");
            this.email = EmailHelper.GetInstance(emailSettings["hostname"], emailSettings["port"].ToInt(), emailSettings["username"], emailSettings["password"], emailSettings["fromaddress"]);
        }

        public void OnGet()
        {

        }
    }
}
