using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIVersioning.DTOs;
using APIVersioning.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIVersioning.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsHeaderVersionController : ControllerBase
    {
         private static readonly List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Laptop", Price = 1200 },
            new Product { Id = 2, Name = "Smartphone", Price = 700 }
        };

        // GET v1
        [HttpGet]
        [ApiVersion("1.0")]
        public IActionResult GetV1() =>
            Ok(_products.Select(p => new ProductResponseV1 { Id = p.Id, Name = p.Name }));

        // GET v2
        [HttpGet]
        [ApiVersion("2.0")]
        public IActionResult GetV2() =>
            Ok(_products.Select(p => new ProductResponseV2 { Id = p.Id, Name = p.Name, Price = p.Price ?? 0 }));

        // POST v1
        [HttpPost]
        [ApiVersion("1.0")]
        public IActionResult PostV1([FromBody] ProductCreateRequestV1 request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Name is required.");
            var newProduct = new Product { Id = _products.Max(p => p.Id) + 1, Name = request.Name, Price = 0 };
            _products.Add(newProduct);
            return Ok(new ProductResponseV1 { Id = newProduct.Id, Name = newProduct.Name });
        }

        // POST v2
        [HttpPost]
        [ApiVersion("2.0")]
        public IActionResult PostV2([FromBody] ProductCreateRequestV2 request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Name is required.");
            if (request.Price <= 0)
                return BadRequest("Price must be greater than zero.");
            var newProduct = new Product { Id = _products.Max(p => p.Id) + 1, Name = request.Name, Price = request.Price };
            _products.Add(newProduct);
            return Ok(newProduct);
        }
    }
}