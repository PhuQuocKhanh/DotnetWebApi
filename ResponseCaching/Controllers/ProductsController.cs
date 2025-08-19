using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResponseCaching.Data;
using ResponseCaching.Model;

namespace ResponseCaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
         private readonly ApplicationDbContext _context;

        // Inject DbContext qua constructor (Dependency Injection)
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        // Cache response trong 60 giây.
        // Tất cả request sẽ dùng chung 1 bản cache.
        [ResponseCache(Duration = 60)]
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            await Task.Delay(TimeSpan.FromSeconds(3)); // giả lập xử lý nặng
            return Ok(products);
        }
                // GET: api/Products
        // Apply the "Default60" cache profile defined in Program.cs.
        [ResponseCache(CacheProfileName = "Default60")]
        [HttpGet("get-products-custom")]
        public async Task<ActionResult<List<Product>>> GetProductsCustom()
        {
            // Retrieve all products from the database
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        // GET: api/Products/5
        // Cache 120 giây, cache tách biệt theo query key "id"
        [ResponseCache(Duration = 120, VaryByQueryKeys = new[] { "id" })]
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            
            await Task.Delay(TimeSpan.FromSeconds(3));
            return Ok(product);
        }

        // GET: api/Products/vary-query?sort=price
        // Cache 120 giây, thay đổi theo query param "sort"
        [ResponseCache(Duration = 120, VaryByQueryKeys = new[] { "sort" })]
        [HttpGet("vary-query")]
        public async Task<ActionResult<List<Product>>> GetProductsVaryByQuery([FromQuery] string sort)
        {
            var products = await _context.Products.ToListAsync();

            if (!string.IsNullOrEmpty(sort))
            {
                if (sort.Equals("price", StringComparison.OrdinalIgnoreCase))
                    products = products.OrderBy(p => p.Price).ToList();
                else if (sort.Equals("name", StringComparison.OrdinalIgnoreCase))
                    products = products.OrderBy(p => p.Name).ToList();
            }

            await Task.Delay(TimeSpan.FromSeconds(3));
            return Ok(products);
        }

        // GET: api/Products/anylocation
        // Cache response cho cả client và proxy (public)
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        [HttpGet("anylocation")]
        public async Task<ActionResult<List<Product>>> GetProductsAnyLocation()
        {
            var products = await _context.Products.ToListAsync();
            await Task.Delay(TimeSpan.FromSeconds(3));
            return Ok(products);
        }

        // GET: api/Products/clientlocation
        // Cache chỉ trên client (private)
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        [HttpGet("clientlocation")]
        public async Task<ActionResult<List<Product>>> GetProductsClientLocation()
        {
            var products = await _context.Products.ToListAsync();
            await Task.Delay(TimeSpan.FromSeconds(3));
            return Ok(products);
        }

        // GET: api/Products/nolocation/1
        // Cache với Location = None (bắt buộc client phải revalidate)
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.None)]
        [HttpGet("nolocation/{id}")]
        public async Task<ActionResult<List<Product>>> GetProductsNoLocation(int id)
        {
            var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(prd => prd.Id == id);
            if (product == null) return NotFound();

            var lastModified = product.ModifiedDate.ToUniversalTime();
            var eTag = $"{lastModified.Ticks}";

            if (Request.Headers.ContainsKey("If-None-Match") && Request.Headers["If-None-Match"].ToString() == eTag)
                return StatusCode(StatusCodes.Status304NotModified);

            Response.Headers["ETag"] = eTag;
            return Ok(product);
        }

        // GET: api/Products/nostore
        // Không cho phép cache (NoStore = true)
        [ResponseCache(NoStore = true)]
        [HttpGet("nostore")]
        public async Task<ActionResult<List<Product>>> GetProductsNoStore()
        {
            var products = await _context.Products.ToListAsync();
            await Task.Delay(TimeSpan.FromSeconds(3));
            return Ok(products);
        }

        // GET: api/Products/varybyheader
        // Cache thay đổi theo User-Agent header
        [ResponseCache(Duration = 60, VaryByHeader = "User-Agent")]
        [HttpGet("varybyheader")]
        public async Task<ActionResult<List<Product>>> GetProductsVaryByHeader()
        {
            var products = await _context.Products.ToListAsync();
            await Task.Delay(TimeSpan.FromSeconds(3));
            return Ok(products);
        }
    }
}