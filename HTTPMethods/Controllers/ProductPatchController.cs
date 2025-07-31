using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTTPMethods.Patch.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace HTTPMethods.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductPatchController : ControllerBase
    {
        private readonly ProductRepository _productRepository;

        public ProductPatchController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPatch("{Id}")]
        public IActionResult PatchProduct(int Id,
            [FromBody] JsonPatchDocument<Product> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var product = _productRepository.GetById(Id);
            if (product == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(product, error =>
            {
                ModelState.AddModelError(error.AffectedObject?.ToString() ?? string.Empty, error.ErrorMessage);
            });

            if (!TryValidateModel(product))
            {
                return BadRequest(ModelState);
            }

            _productRepository.Update(product);
            return Ok(product);
        }
    }
}