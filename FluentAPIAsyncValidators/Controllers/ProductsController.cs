using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAPIAsyncValidators.Data;
using FluentAPIAsyncValidators.DTOs;
using FluentAPIAsyncValidators.Models;
using FluentAPIAsyncValidators.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FluentAPIAsyncValidators.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ECommerceDbContext _context;
        private readonly IValidator<ProductDTO> _validator;

        // Inject DbContext và Validator qua constructor
        public ProductsController(ECommerceDbContext context, IValidator<ProductDTO> validator)
        {
            _context = context;
            _validator = validator;
        }

        // POST: api/products
        // Tạo mới một product sau khi validate dữ liệu input
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDTO productDTO)
        {
            // Validate request bằng validator đã inject
            var validationResult = await _validator.ValidateAsync(productDTO);

            // Nếu validate fail → trả về 400 BadRequest kèm chi tiết lỗi
            if (!validationResult.IsValid)
            {
                var errorResponse = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                });
                return BadRequest(new { Errors = errorResponse });
            }

            // Map DTO sang Entity Product
            var product = new Product
            {
                Name = productDTO.Name,
                SKU = productDTO.SKU,
                Price = productDTO.Price,
                CategoryId = productDTO.CategoryId,
                Stock = productDTO.Stock,
                Discount = productDTO.Discount,
                Description = productDTO.Description,
            };

            // Thêm vào DbContext và lưu vào DB
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Trả về 200 OK cùng với product vừa tạo
            return Ok(product);
        }

        // POST: api/products
        [HttpPost("product-Manual")]
        public async Task<IActionResult> CreateProductManual([FromBody] ProductDTO productDTO)
        {
            // Tạo validator, truyền DbContext để dùng async validation
            var validator = new ProductDTOValidator(_context);

            // Validate ProductDTO bất đồng bộ
            var validationResult = await validator.ValidateAsync(productDTO);

            // Nếu fail -> trả về 400 Bad Request + error details
            if (!validationResult.IsValid)
            {
                var errorResponse = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                });
                return BadRequest(new { Errors = errorResponse });
            }

            // Map từ DTO sang entity Product
            var product = new Product
            {
                Name = productDTO.Name,
                SKU = productDTO.SKU,
                Price = productDTO.Price,
                CategoryId = productDTO.CategoryId,
                Stock = productDTO.Stock,
                Discount = productDTO.Discount,
                Description = productDTO.Description,
            };

            // Add vào DbContext
            _context.Products.Add(product);

            // Save async
            await _context.SaveChangesAsync();

            // Trả về 200 OK cùng với product vừa tạo
            return Ok(product);
        }
    }
}