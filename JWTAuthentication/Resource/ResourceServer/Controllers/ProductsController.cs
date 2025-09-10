using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceServer.Models;

namespace ResourceServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        // Danh sách trong bộ nhớ để lưu trữ sản phẩm
        private static readonly List<Product> Products = new List<Product>
        {
            new Product { Id = 1, Name = "Product A", Price = 10.0M, Description = "Test Product A" },
            new Product { Id = 2, Name = "Product B", Price = 20.0M, Description = "Test Product B"  },
            new Product { Id = 3, Name = "Product C", Price = 30.0M, Description = "Test Product C"  }
        };
        private static int _nextId = 4; // Để tự động tăng ID sản phẩm

        // Chỉ Admin Role mới được cấp quyền
        // Người dùng phải được xác thực VÀ phải thuộc role "Admin".
        [Authorize(Roles = "Admin")]
        // Người dùng phải được xác thực nhưng có thể thuộc BẤT KỲ role nào hoặc không có role.
        [Authorize]
        [HttpGet("GetAll")]
        public ActionResult<List<Product>> GetAllProduct()
        {
            return Ok(Products);
        }

        // Chỉ User Role mới được cấp quyền
        // Người dùng phải được xác thực VÀ phải thuộc role "User".
        [Authorize(Roles = "User")]
        // Lấy một sản phẩm cụ thể theo ID.
        [HttpGet("GetById/{id}", Name = "GetProductById")]
        public ActionResult<Product> GetProductById(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound(new { message = $"Product with ID {id} not found." });
            }
            return Ok(product);
        }

        // Cấp quyền cho cả "Admin" VÀ "User" Roles
        // Yêu cầu người dùng phải được xác thực và có ít nhất một trong các role trong "UserPolicy".
        // Có thể truy cập bởi các role Admin, Editor, hoặc User.
        [Authorize(Policy = "UserPolicy")]
        // Tạo một sản phẩm mới.
        [HttpPost("Add")]
        public IActionResult AddProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            product.Id = _nextId++;
            Products.Add(product);
            return CreatedAtRoute("GetProductById", new { id = product.Id }, product);
        }

        // Cấp quyền cho cả "Admin" VÀ "User" Roles
        // Yêu cầu người dùng phải được xác thực và có ít nhất một trong các role trong "EditorPolicy".
        // Chỉ có thể truy cập bởi các role Admin hoặc Editor.
        [Authorize(Policy = "EditorPolicy")] 
        // Cập nhật một sản phẩm hiện có.
        [HttpPut("Update/{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingProduct = Products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                return NotFound(new { message = $"Product with ID {id} not found." });
            }
            existingProduct.Name = updatedProduct.Name;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.Price = updatedProduct.Price;
            return NoContent();
        }

        // Cấp quyền cho cả hai role Admin và User
       // Yêu cầu người dùng phải được xác thực và có role được định nghĩa trong "AdminPolicy".
        // Chỉ có Admin mới có thể truy cập endpoint này.
        [Authorize(Policy = "AdminPolicy")]
        // Xóa một sản phẩm theo ID.
        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound(new { message = $"Product with ID {id} not found." });
            }
            Products.Remove(product);
            return NoContent();
        }
    }
}