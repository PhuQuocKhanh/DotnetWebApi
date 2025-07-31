using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTTPMethods.Data;
using HTTPMethods.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HTTPMethods.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductDbContext _context;

        public ProductsController(ProductDbContext context)
        {
            _context = context;
        }

        // GET: Lấy danh sách tất cả sản phẩm (Safe, Idempotent)
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _context.Products.ToListAsync();
                return Ok(products); // Trả về 200 OK kèm danh sách sản phẩm
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi khi lấy danh sách sản phẩm.", Details = ex.Message });
            }
        }

        // GET: Lấy sản phẩm theo ID (Safe, Idempotent)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return NotFound(new { Message = $"Không tìm thấy sản phẩm có ID = {id}." });

                return Ok(product); // Trả về 200 OK kèm thông tin sản phẩm
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi khi lấy thông tin sản phẩm.", Details = ex.Message });
            }
        }

        // POST: Tạo sản phẩm mới (Unsafe, Non-Idempotent)
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState); // Trả về 400 Bad Request nếu model không hợp lệ

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product); // Trả về 201 Created
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi khi tạo sản phẩm.", Details = ex.Message });
            }
        }

        // PUT: Cập nhật toàn bộ sản phẩm (Unsafe, Idempotent)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            try
            {
                if (id != product.Id)
                    return BadRequest("ID sản phẩm không khớp."); // 400 Bad Request

                var existingProduct = await _context.Products.FindAsync(id);
                if (existingProduct == null)
                    return NotFound(new { Message = $"Không tìm thấy sản phẩm có ID = {id}." });

                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;

                await _context.SaveChangesAsync();

                return NoContent(); // Trả về 204 No Content
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { Message = "Lỗi khi cập nhật sản phẩm.", Details = ex.InnerException?.Message ?? ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi không xác định khi cập nhật sản phẩm.", Details = ex.Message });
            }
        }

        // PATCH: Cập nhật một phần (giá sản phẩm) (Unsafe, Idempotent tùy cách dùng)
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateProductPrice(int id, [FromBody] Product product)
        {
            try
            {
                if (id != product.Id)
                    return BadRequest("ID sản phẩm không khớp.");

                var existingProduct = await _context.Products.FindAsync(id);
                if (existingProduct == null)
                    return NotFound(new { Message = $"Không tìm thấy sản phẩm có ID = {id}." });

                existingProduct.Price = product.Price;

                await _context.SaveChangesAsync();
                return NoContent(); // 204 No Content
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { Message = "Lỗi khi cập nhật giá sản phẩm.", Details = ex.InnerException?.Message ?? ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi không xác định khi cập nhật giá.", Details = ex.Message });
            }
        }

        // DELETE: Xóa sản phẩm (Unsafe, Idempotent)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return NotFound(new { Message = $"Không tìm thấy sản phẩm có ID = {id}." });

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return NoContent(); // 204 No Content
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { Message = "Lỗi khi xóa sản phẩm.", Details = ex.InnerException?.Message ?? ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi không xác định khi xóa sản phẩm.", Details = ex.Message });
            }
        }

        // HEAD: Trả về metadata (header) sản phẩm, không bao gồm body (Safe, Idempotent)
        [HttpHead("{id}")]
        public async Task<IActionResult> HeadProduct(int id)
        {
            try
            {
                var exists = await _context.Products.AnyAsync(p => p.Id == id);
                if (!exists)
                    return NotFound();

                Response.Headers.Append("Content-Type", "application/json");

                var contentLength = System.Text.Json.JsonSerializer.Serialize(exists).Length;
                Response.Headers.Append("Content-Length", contentLength.ToString());

                Response.Headers.Append("X-Custom-Header", "CustomHeaderValue");

                return Ok(); // Trả về 200 OK với chỉ header
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi khi lấy metadata sản phẩm.", Details = ex.Message });
            }
        }

        // OPTIONS: Trả về danh sách HTTP method được hỗ trợ (Safe, Idempotent)
        [HttpOptions]
        public IActionResult GetOptions()
        {
            try
            {
                Response.Headers.Append("Allow", "GET, POST, PUT, PATCH, DELETE, OPTIONS, HEAD");
                return Ok(); // Trả về 200 OK với header Allow
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi khi xử lý OPTIONS.", Details = ex.Message });
            }
        }
    }
}