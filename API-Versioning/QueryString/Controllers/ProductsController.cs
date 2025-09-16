using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QueryString.Models;

namespace QueryString.Controllers
{
    [Route("api/products")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ProductsV1Controller : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "Apple", Price = 1.50, Category = "Fruit" },
                new Product { Id = 2, Name = "Bread", Price = 2.25, Category = "Bakery" }
            };
        }

        [HttpGet]
        [ApiVersion("1.0")]
        [Route("{Id}")]
        public Product GetGetProductsById(int Id)
        {
            return new Product { Id = Id, Name = "Apple", Price = 1.50, Category = "Fruit" };
        }
    }

    [Route("api/products")]
    [ApiController]
    [ApiVersion("2.0")]
    public class ProductsV2Controller : ControllerBase
    {
        [HttpGet]
        [ApiVersion("2.0")]
        public IEnumerable<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "Apple", Price = 1.50, Category = "Fruit" },
                new Product { Id = 2, Name = "Bread", Price = 2.25, Category = "Bakery" },
                new Product { Id = 3, Name = "Carrot", Price = 0.75, Category = "Vegetable" }
            };
        }
    }
}