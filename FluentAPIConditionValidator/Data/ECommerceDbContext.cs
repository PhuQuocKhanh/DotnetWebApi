using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAPIConditionValidator.Models;
using Microsoft.EntityFrameworkCore;

namespace FluentAPIConditionValidator.Data
{
    // Đại diện cho DbContext của Entity Framework Core cho ứng dụng thương mại điện tử.
    public class ECommerceDbContext : DbContext
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options)
            : base(options)
        {
        }
        // Khởi tạo một số dữ liệu ban đầu cho khách hàng
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasData(
                new Customer { CustomerId = 1, Name = "John Doe"},
                new Customer { CustomerId = 2, Name = "Jane Smith" },
                new Customer { CustomerId = 3, Name = "Emily Johnson"}
            );
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}