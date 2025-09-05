using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BasicAuthenticationDemo.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed dữ liệu User (dữ liệu mồi)
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FullName = "Pranaya Rout", Email = "pranaya.rout@example.com", PasswordHash = PasswordHasher.HashPassword("Pranaya@123"), Role = "Administrator,Manager" },
                new User { Id = 2, FullName = "John Doe", Email = "john.doe@example.com", PasswordHash = PasswordHasher.HashPassword("John@123"), Role = "Administrator" },
                new User { Id = 3, FullName = "Jane Smith", Email = "jane.smith@example.com", PasswordHash = PasswordHasher.HashPassword("Jane@123"), Role = "Manager" }
            );

            // Seed dữ liệu Product
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Description = "High performance laptop", Price = 1200.00m, Stock = 15 },
                new Product { Id = 2, Name = "Wireless Mouse", Description = "Ergonomic wireless mouse", Price = 25.99m, Stock = 100 },
                new Product { Id = 3, Name = "Mechanical Keyboard", Description = "RGB backlit mechanical keyboard", Price = 79.99m, Stock = 45 }
            );
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
    }
}