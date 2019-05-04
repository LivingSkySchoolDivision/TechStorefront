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
            ProductRepository productRepo = new ProductRepository("ConnectionStringWouldGoHere");

            List<Product> allProducts = productRepo.GetAll();

            Console.WriteLine("Found " + allProducts.Count +"  products:");
            foreach(Product product in allProducts)
            {
                Console.WriteLine("> " + product.Id + ": " + product.Name);
            }
            
        }
    }
}
