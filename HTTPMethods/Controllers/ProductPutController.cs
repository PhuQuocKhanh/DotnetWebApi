using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTTPMethods.PUT.Models;
using Microsoft.AspNetCore.Mvc;

namespace HTTPMethods.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductPutController(ProductPUTRepository productRepository) : ControllerBase
    {
        private readonly ProductPUTRepository _productRepository = productRepository;

        // Cập nhật thông tin sản phẩm theo ID
        // PUT: api/Product/1
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductPUT>> PutProduct(int id, [FromBody] ProductPUT product)
        {
            if (id != product.Id)
            {
                return BadRequest(); // Trả về lỗi nếu ID không khớp
            }

            var result = await _productRepository.UpdateProductAsync(product);

            if (result == null)
            {
                return NotFound(); // Trả về lỗi nếu không tìm thấy sản phẩm
            }

            return Ok(result); // Trả về sản phẩm đã cập nhật
        }
    }
}