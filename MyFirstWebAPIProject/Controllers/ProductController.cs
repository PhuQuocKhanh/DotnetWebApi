using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFirstWebAPIProject.Models;

namespace MyFirstWebAPIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        // Danh sách sản phẩm trong bộ nhớ (cho mục đích minh họa)
        // Trong thực tế, chúng ta sẽ sử dụng cơ sở dữ liệu
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 1000.00m, Category = "Electronics" },
            new Product { Id = 2, Name = "Desktop", Price = 2000.00m, Category = "Electronics" },
            new Product { Id = 3, Name = "Mobile", Price = 300.00m, Category = "Electronics" },
            // Có thể thêm các sản phẩm khác ở đây
        };

        // GET: api/products
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            return Ok(_products);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound(new { Message = $"Không tìm thấy sản phẩm với ID {id}" });
            }
            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public ActionResult<Product> PostProduct([FromBody] Product product)
        {
            // Logic gán ID cơ bản (cho mục đích minh họa)
            product.Id = _products.Max(p => p.Id) + 1;
            _products.Add(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public IActionResult PutProduct(int id, [FromBody] Product updatedProduct)
        {
            if (id != updatedProduct.Id)
            {
                return BadRequest(new { Message = "ID không khớp giữa route và body" });
            }

            var existingProduct = _products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                return NotFound(new { Message = $"Không tìm thấy sản phẩm với ID {id}" });
            }

            // Cập nhật các thuộc tính sản phẩm
            existingProduct.Name = updatedProduct.Name;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.Category = updatedProduct.Category;

            // Trong ứng dụng thực tế, lưu thay đổi vào CSDL ở đây
            return NoContent();
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound(new { Message = $"Không tìm thấy sản phẩm với ID {id}" });
            }

            _products.Remove(product);
            // Trong ứng dụng thực tế, xóa sản phẩm khỏi CSDL ở đây
            return NoContent();
        }
    }
}