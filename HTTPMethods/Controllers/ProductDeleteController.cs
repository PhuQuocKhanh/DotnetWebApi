using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTTPMethods.Delete.Models;
using Microsoft.AspNetCore.Mvc;

namespace HTTPMethods.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductDeleteController : ControllerBase
    {
        private readonly ProductRepository _productRepository;

        public ProductDeleteController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // DELETE: api/Product/1
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteProduct(int Id)
        {
            var product = await _productRepository.GetByIdAsync(Id);
            if (product == null)
            {
                return NotFound();
            }

            await _productRepository.DeleteProductAsync(product);
            return NoContent(); // Trả về mã 204 khi xóa thành công
        }
    }
}