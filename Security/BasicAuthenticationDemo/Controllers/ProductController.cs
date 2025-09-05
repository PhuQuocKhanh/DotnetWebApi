using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicAuthenticationDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicAuthenticationDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/product
        // Endpoint công khai - Không yêu cầu xác thực
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<ProductResponseDTO>>> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        // GET: api/product/{id}
        // Bất kỳ người dùng đã xác thực nào (không yêu cầu vai trò cụ thể)
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductResponseDTO>> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound(new { Message = $"Product with Id = {id} not found." });
            return Ok(product);
        }

        // POST: api/product
        // Yêu cầu một vai trò duy nhất: Administrator
        [Authorize(AuthenticationSchemes = "BasicAuthentication", Roles = "Administrator")]
        [HttpPost]
        public async Task<ActionResult<ProductResponseDTO>> Create(CreateProductDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var createdProduct = await _productService.CreateAsync(dto);
            return Ok(createdProduct);
        }

        // PUT: api/product/{id}
        // Yêu cầu nhiều vai trò (hoặc): Administrator hoặc Manager
        [Authorize(AuthenticationSchemes = "BasicAuthentication", Roles = "Administrator,Manager")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateProductDTO dto)
        {
            if (id != dto.Id)
                return BadRequest(new { Message = "Id in URL and payload must match." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _productService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new { Message = $"Product with Id = {id} not found." });

            return NoContent();
        }

        // DELETE: api/product/{id}
        // Yêu cầu nhiều vai trò (và): Cả Administrator và Manager
        [Authorize(AuthenticationSchemes = "BasicAuthentication", Roles = "Administrator")]
        [Authorize(AuthenticationSchemes = "BasicAuthentication", Roles = "Manager")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _productService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Message = $"Product with Id = {id} not found." });

            return NoContent();
        }
    }
}