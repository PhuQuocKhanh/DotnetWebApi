using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAPIValidation.Data;
using FluentAPIValidation.DTOs;
using FluentAPIValidation.Models;
using FluentAPIValidation.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FluentAPIValidation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ECommerceDbContext _context;

        // Inject DbContext qua constructor
        public ProductsController(ECommerceDbContext context)
        {
            _context = context;
        }

        // GET: api/products?tags=tag1,tag2
        // Trả về tất cả sản phẩm, có thể lọc theo tags.
        [HttpGet]
        public async Task<ActionResult<ProductResponseDTO>> GetProducts([FromQuery] string? tags)
        {
            IQueryable<Product> query = _context.Products
                .AsNoTracking()
                .Include(p => p.Tags);

            if (!string.IsNullOrEmpty(tags))
            {
                var tagList = tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                  .Select(t => t.Trim().ToLower())
                                  .ToList();

                query = query.Where(p => p.Tags.Any(t => tagList.Contains(t.Name.ToLower())));
            }

            var products = await query.ToListAsync();

            var result = products.Select(p => new ProductResponseDTO
            {
                ProductId = p.ProductId,
                SKU = p.SKU,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
                CategoryId = p.CategoryId,
                Description = p.Description,
                Discount = p.Discount,
                ManufacturingDate = p.ManufacturingDate,
                ExpiryDate = p.ExpiryDate,
                Tags = p.Tags.Select(t => t.Name).ToList()
            }).ToList();

            return Ok(result);
        }

        // GET: api/products/{id}
        // Trả về chi tiết một sản phẩm theo ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDTO>> GetProduct(int id)
        {
            var product = await _context.Products
                .AsNoTracking()
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
                return NotFound();

            var response = new ProductResponseDTO
            {
                ProductId = product.ProductId,
                SKU = product.SKU,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Discount = product.Discount,
                ManufacturingDate = product.ManufacturingDate,
                ExpiryDate = product.ExpiryDate,
                Tags = product.Tags.Select(t => t.Name).ToList()
            };

            return Ok(response);
        }

        // POST: api/products
        // Tạo mới sản phẩm từ ProductCreateDTO.
        [HttpPost]
        public async Task<ActionResult<ProductResponseDTO>> CreateProduct([FromBody] ProductCreateDTO productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = new Product
            {
                SKU = productDto.SKU,
                Name = productDto.Name,
                Price = productDto.Price,
                Stock = productDto.Stock,
                CategoryId = productDto.CategoryId,
                Description = productDto.Description,
                Discount = productDto.Discount,
                ManufacturingDate = productDto.ManufacturingDate,
                ExpiryDate = productDto.ExpiryDate
            };

            if (productDto.Tags != null && productDto.Tags.Any())
            {
                foreach (var tagName in productDto.Tags)
                {
                    var normalizedTagName = tagName.Trim().ToLower();
                    var existingTag = await _context.Tags
                        .FirstOrDefaultAsync(t => t.Name.ToLower() == normalizedTagName);

                    if (existingTag != null)
                        product.Tags.Add(existingTag);
                    else
                        product.Tags.Add(new Tag { Name = normalizedTagName });
                }
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var response = new ProductResponseDTO
            {
                ProductId = product.ProductId,
                SKU = product.SKU,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Discount = product.Discount,
                ManufacturingDate = product.ManufacturingDate,
                ExpiryDate = product.ExpiryDate,
                Tags = product.Tags.Select(t => t.Name).ToList()
            };

            return Ok(response);
        }

        // PUT: api/products/{id}
        // Cập nhật sản phẩm từ ProductUpdateDTO.
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductResponseDTO>> UpdateProduct(int id, [FromBody] ProductUpdateDTO productDto)
        {
            if (id != productDto.ProductId)
                return BadRequest(new { error = "Product ID trong URL và body không khớp." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _context.Products
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
                return NotFound();

            product.SKU = productDto.SKU;
            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Stock = productDto.Stock;
            product.CategoryId = productDto.CategoryId;
            product.Description = productDto.Description;
            product.Discount = productDto.Discount;
            product.ManufacturingDate = productDto.ManufacturingDate;
            product.ExpiryDate = productDto.ExpiryDate;

            product.Tags.Clear();

            if (productDto.Tags != null && productDto.Tags.Any())
            {
                foreach (var tagName in productDto.Tags)
                {
                    var normalizedTagName = tagName.Trim().ToLower();
                    var existingTag = await _context.Tags
                        .FirstOrDefaultAsync(t => t.Name.ToLower() == normalizedTagName);

                    if (existingTag != null)
                        product.Tags.Add(existingTag);
                    else
                        product.Tags.Add(new Tag { Name = normalizedTagName });
                }
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            var response = new ProductResponseDTO
            {
                ProductId = product.ProductId,
                SKU = product.SKU,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Discount = product.Discount,
                ManufacturingDate = product.ManufacturingDate,
                ExpiryDate = product.ExpiryDate,
                Tags = product.Tags.Select(t => t.Name).ToList()
            };

            return Ok(response);
        }

        [HttpPost("product-manual")]
        public async Task<ActionResult<ProductResponseDTO>> CreateProductManual([FromBody] ProductCreateDTO productDto)
        {
            // Khởi tạo validator và validate DTO
            var validator = new ProductCreateDTOValidator();
            var validationResult = validator.Validate(productDto);

            // Cách 1: Trả về mặc định danh sách lỗi
            // if (!validationResult.IsValid)
            // {
            //     return BadRequest(validationResult.Errors);
            // }

            // Cách 2: Trả về lỗi custom
            if (!validationResult.IsValid)
            {
                var errorResponse = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                });

                return BadRequest(new { Errors = errorResponse });
            }

            // Nếu valid thì mapping DTO -> Entity
            var product = new Product
            {
                SKU = productDto.SKU,
                Name = productDto.Name,
                Price = productDto.Price,
                Stock = productDto.Stock,
                CategoryId = productDto.CategoryId,
                Description = productDto.Description,
                Discount = productDto.Discount,
                ManufacturingDate = productDto.ManufacturingDate,
                ExpiryDate = productDto.ExpiryDate
            };

            // Xử lý Tags: kiểm tra tồn tại, nếu chưa có thì tạo mới
            if (productDto.Tags != null && productDto.Tags.Any())
            {
                foreach (var tagName in productDto.Tags)
                {
                    var normalizedTagName = tagName.Trim().ToLower();
                    var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name.ToLower() == normalizedTagName);

                    if (existingTag != null)
                        product.Tags.Add(existingTag);
                    else
                        product.Tags.Add(new Tag { Name = normalizedTagName });
                }
            }

            // Thêm vào DB
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Mapping sang DTO response
            var response = new ProductResponseDTO
            {
                ProductId = product.ProductId,
                SKU = product.SKU,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Discount = product.Discount,
                ManufacturingDate = product.ManufacturingDate,
                ExpiryDate = product.ExpiryDate,
                Tags = product.Tags.Select(t => t.Name).ToList()
            };

            return Ok(response);
        }
    }
}