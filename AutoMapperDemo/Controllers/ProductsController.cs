using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapperDemo.Data;
using AutoMapperDemo.DTOs;
using AutoMapperDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoMapperDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductDbContext _context;
        private readonly IMapper _mapper;
        public ProductsController(ProductDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: api/Products/GetProducts
        [HttpGet("GetProducts")]
        public async Task<ActionResult<List<ProductDTO>>> GetProducts()
        {
            var products = await _context.Products.AsNoTracking().ToListAsync();
            // AutoMapper automatically maps the list of Products to a list of ProductDTOs
            var productDTOs = _mapper.Map<List<ProductDTO>>(products);
            return Ok(productDTOs);
        }
        // GET: api/Products/GetProductbyId/{id}
        [HttpGet("GetProductbyId/{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductbyId(int id)
        {
            var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(prd => prd.Id == id);
            if (product == null)
                return NotFound();
            // AutoMapper mapping from Product to ProductDTO
            var productDTO = _mapper.Map<ProductDTO>(product);
            return Ok(productDTO);
        }
        // POST: api/Products/AddProduct
        [HttpPost("AddProduct")]
        public async Task<ActionResult<ProductDTO>> AddProduct(ProductCreateDTO productCreateDTO)
        {
            if (productCreateDTO == null)
                return BadRequest("Invalid product data.");
            // Map the DTO to a Product entity
            var product = _mapper.Map<Product>(productCreateDTO);
            // Determine availability based on stock quantity
            product.IsAvailable = product.StockQuantity > 0;
            // CreatedDate as Cuurent Date
            product.CreatedDate = DateTime.Now;
            // First, add the product without SKU to generate the Product.Id
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            // Now that the product.Id is generated, create the SKU using the new logic:
            product.SKU = GenerateSKU(product);
            // Update the product record with the new SKU
            await _context.SaveChangesAsync();
            // AutoMapper mapping from Product to ProductDTO
            var productDTO = _mapper.Map<ProductDTO>(product);
            return Ok(productDTO);
        }
        // Generates a SKU based on:
        //  - First three letters of Category
        //  - First three letters of Brand
        //  - First three letters of Product Name
        //  - Year of CreatedDate
        //  - Product.Id
        // Example: If Category="Electronics", Brand="Samsung", Name="Galaxy", CreatedDate is 2025 and Id is 15, 
        // The SKU would be "ELE-SAM-GAL-2025-15".
        private string GenerateSKU(Product product)
        {
            // Use default values if any fields are missing
            string category = string.IsNullOrWhiteSpace(product.Category) ? "GEN" : product.Category;
            string brand = string.IsNullOrWhiteSpace(product.Brand) ? "BRD" : product.Brand;
            string name = string.IsNullOrWhiteSpace(product.Name) ? "PRD" : product.Name;
            // Extract the first three letters of each, padding if necessary
            string catPrefix = category.Length >= 3
                ? category.Substring(0, 3).ToUpper()
                : category.ToUpper().PadRight(3, 'X');
            string brandPrefix = brand.Length >= 3
                ? brand.Substring(0, 3).ToUpper()
                : brand.ToUpper().PadRight(3, 'X');
            string prodPrefix = name.Length >= 3
                ? name.Substring(0, 3).ToUpper()
                : name.ToUpper().PadRight(3, 'X');
            // Use the year from CreatedDate and the generated Id
            int year = product.CreatedDate.Year;
            int id = product.Id;
            // Assemble SKU with hyphen separators
            return $"{catPrefix}-{brandPrefix}-{prodPrefix}-{year}-{id}";
        }
    }
}