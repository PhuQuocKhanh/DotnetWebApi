using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ResponseCaching.Model;

namespace ResponseCaching.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed dữ liệu mẫu
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Product A", Price = 10.0M, Description = "Product A Description", CreatedDate = DateTime.Today, ModifiedDate = DateTime.Today },
                new Product { Id = 2, Name = "Product B", Price = 20.0M, Description = "Product B Description", CreatedDate = DateTime.Today, ModifiedDate = DateTime.Today },
                new Product { Id = 3, Name = "Product C", Price = 30.0M, Description = "Product C Description", CreatedDate = DateTime.Today, ModifiedDate = DateTime.Today },
                new Product { Id = 4, Name = "Product D", Price = 40.0M, Description = "Product D Description", CreatedDate = DateTime.Today, ModifiedDate = DateTime.Today },
                new Product { Id = 5, Name = "Product E", Price = 50.0M, Description = "Product E Description", CreatedDate = DateTime.Today, ModifiedDate = DateTime.Today }
            );
        }

        public DbSet<Product> Products { get; set; }
    }
}