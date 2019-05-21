using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LSSD.StoreFront.DB;
using LSSD.StoreFront.Lib.UserAccounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace LSSDStoreFront_FrontEnd.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly DatabaseContext dbContext;

        public CheckoutModel(IConfiguration config)
        {
            dbContext = new DatabaseContext(config.GetConnectionString("InternalDatabase"));
        }

        public void OnGet()
        {

        }

        public IActionResult OnPostBackToCart()
        {
            return RedirectToPage("/ShoppingCart");
        }

        public IActionResult OnPostPlaceOrder()
        {
            UserFriendlyShoppingCart shoppingCart = new UserFriendlyShoppingCart(dbContext, User.Identity.Name);
            UserFriendlyOrders orders = new UserFriendlyOrders(dbContext, User.Identity.Name);

            string budgetNumber = Request.Form["txtAccountNum"].ToString() ?? string.Empty;
            string customerNotes = Request.Form["txtCustomerNotes"].ToString() ?? string.Empty;
            string submittedBy = ((System.Security.Claims.ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == "name").FirstOrDefault().Value;
            UserThumbprint userThumb = new UserThumbprint(User.Identity.Name);

            // Create the order
            orders.CreateOrder(shoppingCart.Items, budgetNumber, submittedBy, userThumb, customerNotes);

            // Clear the user's cart 
            shoppingCart.ClearCart();

            // Save the cleared cart
            shoppingCart.Save();

            // Redirect to order thanks page??
            return RedirectToPage("/Orders");
        }
    }
}