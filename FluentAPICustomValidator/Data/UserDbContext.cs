using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAPICustomValidator.Models;
using Microsoft.EntityFrameworkCore;

namespace FluentAPICustomValidator.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Khởi tạo dữ liệu gốc cho Gender
            modelBuilder.Entity<Gender>().HasData(
                new Gender { GenderId = 1, Name = "Male" },
                new Gender { GenderId = 2, Name = "Female" },
                new Gender { GenderId = 3, Name = "Unknown" }
            );

            // Khởi tạo dữ liệu gốc cho Country
            modelBuilder.Entity<Country>().HasData(
                new Country { CountryId = 1, Name = "USA" },
                new Country { CountryId = 2, Name = "India" }
            );

            // Khởi tạo dữ liệu gốc cho City
            modelBuilder.Entity<City>().HasData(
                new City { CityId = 1, Name = "New York", CountryId = 1 },
                new City { CityId = 2, Name = "Los Angeles", CountryId = 1 },
                new City { CityId = 3, Name = "Mumbai", CountryId = 2 },
                new City { CityId = 4, Name = "Delhi", CountryId = 2 }
            );

            // Khởi tạo dữ liệu User ban đầu (bao gồm các thuộc tính bổ sung)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    FirstName = "Pranaya",
                    LastName = "Rout",
                    Email = "pranaya.rout@example.com",
                    Password = "Secure@123",  // Trong production, lưu mật khẩu đã được băm
                    DateOfBirth = new DateTime(1990, 5, 20),
                    PhoneNumber = "9876543210",
                    Address = "123, Main Street",
                    GenderId = 1,
                    CountryId = 2,
                    CityId = 3
                },
                new User
                {
                    UserId = 2,
                    FirstName = "Hina",
                    LastName = "Sharma",
                    Email = "hina.sharma@example.com",
                    Password = "StrongPass@123",
                    DateOfBirth = new DateTime(1985, 8, 15),
                    PhoneNumber = "1234567890",
                    Address = "456, Park Avenue",
                    GenderId = 2,
                    CountryId = 2,
                    CityId = 4
                }
            );
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
    }
}