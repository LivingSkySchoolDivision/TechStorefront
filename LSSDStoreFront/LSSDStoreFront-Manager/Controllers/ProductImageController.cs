using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LSSD.StoreFront.DB;
using LSSD.StoreFront.DB.repositories;
using LSSD.StoreFront.Lib.Products;
using LSSD.StoreFront.Manager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LSSD.StoreFront.Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly DatabaseContext dbContext;

        public ProductImageController(IConfiguration config)
        {
            dbContext = new DatabaseContext(config.GetConnectionString(ManagerSettings.ConnectionStringName));
        }

        // GET: api/ProductImage/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            ProductImageRepository imageRepository = new ProductImageRepository(dbContext);

            ProductImage image = imageRepository.GetImageForProduct(id);
            if (image != null)
            {
                MemoryStream ms = new MemoryStream(image.ImageBytes);
                return File(ms, image.ContentType);
            }

            return NotFound();

        }
    }
}