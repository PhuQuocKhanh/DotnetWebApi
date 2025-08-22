using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAPIAsyncValidators.Models;
using Microsoft.EntityFrameworkCore;

namespace FluentAPIAsyncValidators.Data
{
    public class ECommerceDbContext : DbContext
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed sample data for Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Electronics", Description = "Electronic gadgets and devices" },
                new Category { CategoryId = 2, CategoryName = "Books", Description = "Various genres of books" },
                new Category { CategoryId = 3, CategoryName = "Home Appliances", Description = "Appliances for everyday home use" }
            );
            // Seed sample data for DiscountRules
            modelBuilder.Entity<DiscountRule>().HasData(
                new DiscountRule { DiscountRuleId = 1, MinimumPrice = 100, MaximumDiscount = 10 },
                new DiscountRule { DiscountRuleId = 2, MinimumPrice = 500, MaximumDiscount = 20 },
                new DiscountRule { DiscountRuleId = 3, MinimumPrice = 999, MaximumDiscount = 30 }
            );
            // Seed sample data for Products (including SKU and Stock)
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    Name = "Novel",
                    SKU = "BKN-001",
                    Price = 50,
                    CategoryId = 2,
                    Stock = 100,
                    Discount = 0,
                    Description = "Bestselling novel with an intriguing plot"
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Microwave",
                    SKU = "APPL-001",
                    Price = 150,
                    CategoryId = 3,
                    Stock = 50,
                    Discount = 10,
                    Description = "Compact microwave oven suitable for small kitchens"
                },
                new Product
                {
                    ProductId = 3,
                    Name = "Smartphone",
                    SKU = "ELEC-001",
                    Price = 800,
                    CategoryId = 1,
                    Stock = 30,
                    Discount = 20,
                    Description = "Latest model smartphone with advanced features"
                },
                new Product
                {
                    ProductId = 4,
                    Name = "Laptop",
                    SKU = "ELEC-002",
                    Price = 1200,
                    CategoryId = 1,
                    Stock = 20,
                    Discount = 30,
                    Description = "High-performance laptop for gaming and productivity"
                }
            );
        }
        // DbSet properties for querying and saving instances of each entity.
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<DiscountRule> DiscountRules { get; set; }
    }
}