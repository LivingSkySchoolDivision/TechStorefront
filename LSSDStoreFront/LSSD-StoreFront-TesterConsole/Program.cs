using LSSD.StoreFront.DB;
using LSSD.StoreFront.Lib;
using System;
using System.Collections.Generic;

namespace LSSD_StoreFront_TesterConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set up a random number generator to use later
            Random randomGenerator = new Random();

            // Create our product repository
            // This is a temporary connection string, a real one will be loaded elsewhere
            string connectionString = @"data source=dc-sql2012.lskysd.ca\DCSQL2012;initial catalog=LSKYStorefront;user id=LSSDStoreFront-TEST;password=xyCTEq4tuXBEKPCRgID5dYWUxB8Yklwr;Trusted_Connection=false";
            ProductRepository productRepo = new ProductRepository(connectionString);


            // List all products in the system
            List<Product> allProducts = productRepo.GetAll();

            Console.WriteLine("Found " + allProducts.Count +"  products:");
            foreach(Product product in allProducts)
            {
                Console.WriteLine("> " + product.Id + ": " + product.Name);
            }


            // Try to insert a new product
            Product myNewProduct = new Product()
            {
                CategoryId = 0,
                Name = "My Test Product #" + randomGenerator.Next(100,999).ToString(),
                Description = "A test product",
                Price = (decimal)1.99,
                IsAvailable = true
            };

            productRepo.Create(myNewProduct);

            // Try to modify a product
            allProducts[0].Alert = "This product has extremely low stock!";
            productRepo.Update(allProducts[0]);

            // Try to delete a product
            productRepo.Delete(allProducts[0]);

            // Undelete the product we just deleted
            productRepo.UnDelete(allProducts[0]);
        }
    }
}
