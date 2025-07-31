using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTTPMethods.Models;
using Microsoft.EntityFrameworkCore;

namespace HTTPMethods.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Price = 1000.00M, Description = "A powerful laptop" },
                new Product { Id = 2, Name = "Smartphone", Price = 500.00M, Description = "A modern smartphone" }
            );
        }

        public DbSet<Product> Products { get; set; }
    }
}