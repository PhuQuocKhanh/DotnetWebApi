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
    public class ProductFromBodyController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductFormBody product)
        {
            // Add the product to the database or in-memory store
            // For demonstration, let's return the product back
            return Ok(product);
        }

        [HttpPost("Order")]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            // Process the order
            return Ok(order);
        }
    }
}