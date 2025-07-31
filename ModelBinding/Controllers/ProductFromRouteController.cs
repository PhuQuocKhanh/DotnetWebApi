using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModelBinding.Models;

namespace ModelBinding.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductFromRouteController : ControllerBase
    {
        private static List<ProductFromRoute> Products = new List<ProductFromRoute>
        {
            new() { Id = 1, Name = "Laptop", Categogy = "Electronics", Price = 1000, Quantity = 10 },
            new() { Id = 2, Name = "Desktop", Categogy = "Electronics", Price = 2000, Quantity = 20 },
            new() { Id = 3, Name = "Mobile", Categogy = "Electronics", Price = 3000, Quantity = 30 },
            new() { Id = 4, Name = "Casual Shirts", Categogy = "Apparel", Price = 500, Quantity = 10 },
            new() { Id = 5, Name = "Formal Shirts", Categogy = "Apparel", Price = 600, Quantity = 30 },
            new() { Id = 6, Name = "Jackets & Coats", Categogy = "Apparel", Price = 700, Quantity = 20 },
        };

        [HttpGet("{id}")]
        public IActionResult GetProductById([FromRoute] int id)
        {
            // Logic to retrieve the user by ID
            var product = Products.FirstOrDefault(prd => prd.Id == id);
            if (product != null)
            {
                return Ok(product);
            }
            return NotFound($"No Product Found with Product Id: {id}");
        }

        // Assuming your route template specifies a parameter named "Id"
        // but you want to use "ProductId" within your action method
        [HttpGet("{Id}")]
        public IActionResult GetProductByIdCustomize([FromRoute(Name = "Id")] int ProductId)
        {
            // Now, you can use "ProductId" to refer to the parameter that comes from the route.
            // Fetch the Product by the ProductId and return a response

            var product = Products.FirstOrDefault(prd => prd.Id == ProductId);
            if (product != null)
            {
                return Ok(product);
            }
            return NotFound($"No Product Found with Product Id: {ProductId}");
        }
        
        [HttpGet("{Name}/{Category}")]
        public IActionResult GetProductById([FromRoute] ProductRoute productRoute)
        {
            if(productRoute != null)
            {
                var FilteredProducts = Products.Where(prd => prd.Name.ToLower().StartsWith(productRoute.Name) &&
                prd.Categogy.ToLower() == productRoute?.Category.ToLower()).ToList();
                return Ok(FilteredProducts);
            }
            
            return NotFound($"No Product Found");
        }
    }
}