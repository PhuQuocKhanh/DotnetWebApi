using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ValidateNestedComplex.Models;

namespace ValidateNestedComplex.Data
{
    public class ECommerceDbContext : DbContext
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình mối quan hệ để tránh các đường dẫn xóa chuỗi (multiple cascade paths)
            // Order -> Customer: Tắt cascade delete để ngăn chặn nhiều đường dẫn xóa chuỗi
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); // hoặc DeleteBehavior.NoAction
            // Order -> ShippingAddress: Tắt cascade delete cho mối quan hệ này
            modelBuilder.Entity<Order>()
                .HasOne(o => o.ShippingAddress)
                .WithMany() // Không có thuộc tính điều hướng trên Address vì một địa chỉ có thể được dùng cho nhiều đơn hàng
                .HasForeignKey(o => o.ShippingAddressId)
                .OnDelete(DeleteBehavior.Restrict); // hoặc DeleteBehavior.NoAction
            // Seed dữ liệu cho Customers
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "Alice Johnson", Email = "alice@example.com" },
                new Customer { Id = 2, Name = "Bob Smith", Email = "bob@example.com" }
            );
            // Seed dữ liệu cho Addresses
            modelBuilder.Entity<Address>().HasData(
                new Address { Id = 1, City = "Springfield", State = "IL", ZipCode = "62704", CustomerId = 1 },
                new Address { Id = 2, City = "Shelbyville", State = "IL", ZipCode = "62565", CustomerId = 2 }
            );
            // Seed dữ liệu cho Products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Price = 999.99M, Quantity = 10 },
                new Product { Id = 2, Name = "Smartphone", Price = 499.99M, Quantity = 50 },
                new Product { Id = 3, Name = "Headphones", Price = 79.99M, Quantity = 100 }
            );
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}