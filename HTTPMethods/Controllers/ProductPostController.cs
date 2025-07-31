using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTTPMethods.Post.Models;
using Microsoft.AspNetCore.Mvc;

namespace HTTPMethods.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductPostController(ProductPostRepository productRepository) : ControllerBase
    {
        private readonly ProductPostRepository _productRepository = productRepository;

        // Add a New product
        // POST api/product
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductPost product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Add the product to your data store.
            int ProductId = await _productRepository.AddProductAsync(product);
            product.Id = ProductId;
            // Return a 201 Created response with the created resource
            return CreatedAtAction("GetProduct", new { Id = product.Id }, product);
        }
        // GET method for retrieving a product by ID
        // GET api/product/1
        [HttpGet("{Id}")]
        public async Task<ActionResult<ProductPost>> GetProduct(int Id)
        {
            // Retrieve and return the product from your data store
            var product = await _productRepository.GetByIdAsync(Id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}