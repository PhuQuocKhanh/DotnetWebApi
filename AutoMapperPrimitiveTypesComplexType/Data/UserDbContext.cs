using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapperPrimitiveTypesComplexType.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoMapperPrimitiveTypesComplexType.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed dữ liệu mẫu cho User
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    FirstName = "Pranaya",
                    LastName = "Rout",
                    Email = "pranaya.rout@example.com",
                    PhoneNumber = "123-456-7890"
                }
            );

            // Seed địa chỉ tương ứng với User ở trên
            modelBuilder.Entity<Address>().HasData(
                new Address
                {
                    AddressId = 1,
                    Street = "123 Main St",
                    City = "BBSR",
                    State = "Odisha",
                    ZipCode = "755019",
                    UserId = 1
                }
            );
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}