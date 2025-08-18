using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RedisDemo.Data;
using RedisDemo.Models;

namespace RedisDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _cache;

        // Tiêm ApplicationDbContext và IDistributedCache thông qua constructor.
        public ProductsController(ApplicationDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: api/products/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var cacheKey = "GET_ALL_PRODUCTS";
            List<Product> products;
            try
            {
                // Thử lấy danh sách sản phẩm từ Redis cache.
                var cachedData = await _cache.GetStringAsync(cacheKey);
                if (!string.IsNullOrEmpty(cachedData))
                {
                    // Deserialize JSON string trở lại List<Product>.
                    products = JsonSerializer.Deserialize<List<Product>>(cachedData) ?? new List<Product>();
                }
                else
                {
                    // Cache miss: lấy dữ liệu từ database.
                    products = await _context.Products.AsNoTracking().ToListAsync();
                    if (products != null)
                    {
                        // Serialize danh sách sản phẩm sang JSON string.
                        var serializedData = JsonSerializer.Serialize(products);

                        // Định nghĩa cache options (sử dụng Sliding Expiration).
                        var cacheOptions = new DistributedCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                        // Lưu dữ liệu đã serialize vào Redis.
                        await _cache.SetStringAsync(cacheKey, serializedData, cacheOptions);
                    }
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                // Trả về mã lỗi 500 nếu có exception.
                return StatusCode(500, new { message = "Có lỗi xảy ra khi lấy danh sách sản phẩm.", details = ex.Message });
            }
        }

        // GET: api/products/Category?Category=Fruits
        [HttpGet("Category")]
        public async Task<IActionResult> GetProductByCategory(string Category)
        {
            // Cache key bao gồm category để đảm bảo dữ liệu cache là duy nhất.
            var cacheKey = $"PRODUCTS_{Category}";
            List<Product> products;
            try
            {
                var cachedData = await _cache.GetStringAsync(cacheKey);
                if (!string.IsNullOrEmpty(cachedData))
                {
                    products = JsonSerializer.Deserialize<List<Product>>(cachedData) ?? new List<Product>();
                }
                else
                {
                    // Cache miss: truy vấn DB theo category.
                    products = await _context.Products
                        .Where(prd => prd.Category.ToLower() == Category.ToLower())
                        .AsNoTracking()
                        .ToListAsync();

                    if (products != null)
                    {
                        var serializedData = JsonSerializer.Serialize(products);

                        // Dùng Absolute Expiration để cache tự động hết hạn sau 5 phút.
                        var cacheOptions = new DistributedCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                        await _cache.SetStringAsync(cacheKey, serializedData, cacheOptions);
                    }
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi lấy sản phẩm theo Category.", details = ex.Message });
            }
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            // Cache key cho một sản phẩm cụ thể.
            var cacheKey = $"Product_{id}";
            Product? product;
            try
            {
                var cachedData = await _cache.GetStringAsync(cacheKey);
                if (!string.IsNullOrEmpty(cachedData))
                {
                    product = JsonSerializer.Deserialize<Product>(cachedData) ?? new Product();
                }
                else
                {
                    // Nếu không có trong cache thì lấy từ DB.
                    product = await _context.Products.FindAsync(id);
                    if (product == null)
                        return NotFound($"Không tìm thấy sản phẩm với ID {id}.");

                    var serializedData = JsonSerializer.Serialize(product);
                    await _cache.SetStringAsync(cacheKey, serializedData, new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(5)
                    });
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi lấy sản phẩm.", details = ex.Message });
            }
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            if (id != updatedProduct.Id)
            {
                return BadRequest("Product ID không khớp.");
            }
            try
            {
                var existingProduct = await _context.Products.FindAsync(id);
                if (existingProduct == null)
                    return NotFound($"Không tìm thấy sản phẩm với ID {id}.");

                // Cập nhật dữ liệu sản phẩm trong DB.
                _context.Entry(existingProduct).CurrentValues.SetValues(updatedProduct);
                await _context.SaveChangesAsync();

                // Đồng bộ lại cache cho sản phẩm này.
                var cacheKey = $"Product_{id}";
                var serializedData = JsonSerializer.Serialize(updatedProduct);
                await _cache.SetStringAsync(cacheKey, serializedData, new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi cập nhật sản phẩm.", details = ex.Message });
            }
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return NotFound($"Không tìm thấy sản phẩm với ID {id}.");

                // Xóa sản phẩm khỏi DB.
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                // Xóa sản phẩm khỏi cache.
                var cacheKey = $"Product_{id}";
                await _cache.RemoveAsync(cacheKey);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi xóa sản phẩm.", details = ex.Message });
            }
        }
    }
}