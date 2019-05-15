using LSSD.StoreFront.DB;
using LSSD.StoreFront.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSSD_StoreFront_TesterConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // set up a database connection
            string connectionString = @"data source=dc-sql2012.lskysd.ca\DCSQL2012;initial catalog=LSKYStorefront;user id=LSSDStoreFront-TEST;password=xyCTEq4tuXBEKPCRgID5dYWUxB8Yklwr;Trusted_Connection=false";
            DatabaseContext dbConnection = new DatabaseContext(connectionString);
            User User = new User()
            {
                Id = 0,
                Username = "Test User"
            };

            Inventory Inventory = new Inventory(dbConnection);
            ShoppingCart ShoppingCart = new ShoppingCart(dbConnection, User);
            
            foreach (ShoppingCartItem p in ShoppingCart.Items)
            {
                Console.WriteLine(p.Quantity + " " + p.Product.Name);
            }

            ShoppingCart.ClearCart();

            foreach (ShoppingCartItem p in ShoppingCart.Items)
            {
                Console.WriteLine(p.Quantity + " " + p.Product.Name);
            }

            ShoppingCart.Save();

            foreach (ShoppingCartItem p in ShoppingCart.Items)
            {
                Console.WriteLine(p.Quantity + " " + p.Product.Name);
            }
        }
    }
}
