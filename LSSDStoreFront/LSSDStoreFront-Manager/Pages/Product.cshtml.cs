﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LSSD.StoreFront.Lib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LSSD.StoreFront.DB;
using Microsoft.Extensions.Configuration;
using LSSD.StoreFront.DB.repositories;
using System.Web;

namespace LSSD.StoreFront.Manager.Pages
{
    public class ProductModel : PageModel
    {
        DatabaseContext dbContext;

        public ProductModel(IConfiguration config)
        {
            dbContext = new DatabaseContext(config.GetConnectionString(ManagerSettings.ConnectionStringName));
        }
        
        public void OnPostUpdateItem()
        {
            Product updatedProduct = new Product()
            {
                Id = Request.Form["txtProductId"].ToString().ToInt(),
                Name = Request.Form["txtProductName"].ToString(),
                Description = HttpUtility.HtmlEncode(Request.Form["txtShortDescription"].ToString()),
                LongDescription = HttpUtility.HtmlEncode(Request.Form["txtLongDescription"].ToString()),
                CategoryId = Request.Form["drpCategoryId"].ToString().ToInt(),
                ThumbnailFileName = Request.Form["txtThumbnail"].ToString(),
                BasePrice = Request.Form["txtBasePrice"].ToString().ToDecimal(),
                RecyclingFee = Request.Form["txtRecyclingFee"].ToString().ToDecimal(),
                IsGSTExempt = Request.Form["chkGSTExempt"].ToString().ToBool(),
                IsPSTExempt = Request.Form["chkPSTExempt"].ToString().ToBool(),
                IsAvailable = Request.Form["chkIsAvailable"].ToString().ToBool(),
            };
            
            ProductRepository productRepository = new ProductRepository(dbContext);

            productRepository.Update(updatedProduct);

        }

        public IActionResult OnPostDeleteProduct()
        {
            Product updatedProduct = new Product()
            {
                Id = Request.Form["txtProductId"].ToString().ToInt()
            };

            ProductRepository productRepository = new ProductRepository(dbContext);
            productRepository.Delete(updatedProduct);

            return RedirectToPage("/Products");
        }
    }
}