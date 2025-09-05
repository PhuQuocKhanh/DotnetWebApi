using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BasicAuthenticationDemo.Models
{
    public interface IProductService
    {
        Task<List<ProductResponseDTO>> GetAllAsync();
        Task<ProductResponseDTO?> GetByIdAsync(int id);
        Task<ProductResponseDTO> CreateAsync(CreateProductDTO dto);
        Task<bool> UpdateAsync(int id, UpdateProductDTO dto);
        Task<bool> DeleteAsync(int id);
    }
    
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Ánh xạ Product -> ProductDto
        private static ProductResponseDTO MapToDto(Product product)
        {
            return new ProductResponseDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock
            };
        }

        public async Task<List<ProductResponseDTO>> GetAllAsync()
        {
            var products = await _context.Products.ToListAsync();
            return products.Select(MapToDto).ToList();
        }

        public async Task<ProductResponseDTO?> GetByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product == null ? null : MapToDto(product);
        }

        public async Task<ProductResponseDTO> CreateAsync(CreateProductDTO dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return MapToDto(product);
        }

        public async Task<bool> UpdateAsync(int id, UpdateProductDTO dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            if (dto.Name != null) product.Name = dto.Name;
            if (dto.Description != null) product.Description = dto.Description;
            if (dto.Price.HasValue) product.Price = dto.Price.Value;
            if (dto.Stock.HasValue) product.Stock = dto.Stock.Value;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}