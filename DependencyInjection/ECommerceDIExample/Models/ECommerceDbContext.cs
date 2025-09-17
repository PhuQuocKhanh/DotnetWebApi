using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ECommerceDIExample.Models
{
    /// The application's database context with DbSet properties and seed data.
    public class ECommerceDbContext : DbContext
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options)
            : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        // Configures the model with seed data.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Define static dates for seed data
            var staticCreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var staticUpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "john.doe@example.com",
                    Name = "John Doe",
                    CreatedAt = staticCreatedAt,
                    UpdatedAt = staticUpdatedAt,
                    IsActive = true,
                    UserType = "VIP"
                },
                new User
                {
                    Id = 2,
                    Email = "jane.smith@example.com",
                    Name = "Jane Smith",
                    CreatedAt = staticCreatedAt,
                    UpdatedAt = staticUpdatedAt,
                    IsActive = true,
                    UserType = "Premium"
                }
            );
            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Wireless Mouse",
                    Description = "Ergonomic wireless mouse with adjustable DPI.",
                    Price = 25.99m,
                    Stock = 150,
                    CreatedAt = staticCreatedAt,
                    UpdatedAt = staticUpdatedAt,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 2,
                    Name = "Mechanical Keyboard",
                    Description = "RGB backlit mechanical keyboard with blue switches.",
                    Price = 89.99m,
                    Stock = 80,
                    CreatedAt = staticCreatedAt,
                    UpdatedAt = staticUpdatedAt,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 3,
                    Name = "HD Monitor",
                    Description = "24-inch full HD monitor with IPS panel.",
                    Price = 149.99m,
                    Stock = 60,
                    CreatedAt = staticCreatedAt,
                    UpdatedAt = staticUpdatedAt,
                    IsAvailable = true
                }
            );
            // Seed Carts
            modelBuilder.Entity<Cart>().HasData(
                new Cart
                {
                    Id = 1,
                    UserId = 1,
                    CreatedAt = staticCreatedAt,
                    UpdatedAt = staticUpdatedAt,
                    IsCheckedOut = false
                },
                new Cart
                {
                    Id = 2,
                    UserId = 2,
                    CreatedAt = staticCreatedAt,
                    UpdatedAt = staticUpdatedAt,
                    IsCheckedOut = false
                }
            );
            // Seed CartItems
            modelBuilder.Entity<CartItem>().HasData(
                new CartItem
                {
                    Id = 1,
                    CartId = 1,
                    ProductId = 1,
                    Quantity = 2,
                    UnitPrice = 25.99m,
                    TotalPrice = 51.98m,
                    CreatedAt = staticCreatedAt,
                    UpdatedAt = staticUpdatedAt
                },
                new CartItem
                {
                    Id = 2,
                    CartId = 1,
                    ProductId = 2,
                    Quantity = 1,
                    UnitPrice = 89.99m,
                    TotalPrice = 89.99m,
                    CreatedAt = staticCreatedAt,
                    UpdatedAt = staticUpdatedAt
                },
                new CartItem
                {
                    Id = 3,
                    CartId = 2,
                    ProductId = 3,
                    Quantity = 1,
                    UnitPrice = 149.99m,
                    TotalPrice = 149.99m,
                    CreatedAt = staticCreatedAt,
                    UpdatedAt = staticUpdatedAt
                }
            );
        }
    }
}