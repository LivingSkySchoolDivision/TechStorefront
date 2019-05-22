using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LSSD.StoreFront.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace LSSD.StoreFront.FrontEnd.Pages
{
    public class ItemModel : PageModel
    {
        private readonly DatabaseContext dbContext;

        public void OnGet()
        {
        }

        public ItemModel(IConfiguration config)
        {
            dbContext = new DatabaseContext(config.GetConnectionString(FrontendSettings.ConnectionStringName));
        }

        public void OnPostAddItemToCart()
        {
            int productId = (Request.Form["txtProductId"].ToString() ?? string.Empty).ToInt();
            if (productId > 0)
            {
                int quantity = (Request.Form["txtQuantity"].ToString() ?? string.Empty).ToInt();
                if (quantity > 0) {
                    UserFriendlyShoppingCart shoppingCart = new UserFriendlyShoppingCart(dbContext, User.Identity.Name);
                    shoppingCart.AddItem(productId, quantity);
                    shoppingCart.Save();
                }
            }
        }
    }

}