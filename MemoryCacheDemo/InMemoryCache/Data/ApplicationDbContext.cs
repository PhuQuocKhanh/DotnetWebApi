using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemoryCacheDemo.Model.InMemoryCache;
using Microsoft.EntityFrameworkCore;

namespace MemoryCacheDemo.InMemoryCache.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor nhận DbContextOptions và truyền cho lớp cơ sở
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        // Ghi đè OnModelCreating để seed dữ liệu khởi tạo
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed dữ liệu Country
            modelBuilder.Entity<Country>().HasData(
                new Country { CountryId = 1, Name = "India" },
                new Country { CountryId = 2, Name = "United States" },
                new Country { CountryId = 3, Name = "Canada" },
                new Country { CountryId = 4, Name = "United Kingdom" }
            );

            // Seed dữ liệu State
            modelBuilder.Entity<State>().HasData(
                new State { StateId = 1, Name = "California", CountryId = 2 },
                new State { StateId = 2, Name = "Texas", CountryId = 2 },
                new State { StateId = 3, Name = "British Columbia", CountryId = 3 },
                new State { StateId = 4, Name = "Ontario", CountryId = 3 },
                new State { StateId = 5, Name = "England", CountryId = 4 },
                new State { StateId = 6, Name = "Maharashtra", CountryId = 1 },
                new State { StateId = 7, Name = "Delhi", CountryId = 1 }
            );

            // Seed dữ liệu City
            modelBuilder.Entity<City>().HasData(
                new City { CityId = 1, Name = "Los Angeles", StateId = 1 },
                new City { CityId = 2, Name = "San Francisco", StateId = 1 },
                new City { CityId = 3, Name = "Houston", StateId = 2 },
                new City { CityId = 4, Name = "Dallas", StateId = 2 },
                new City { CityId = 5, Name = "Vancouver", StateId = 3 },
                new City { CityId = 6, Name = "Toronto", StateId = 4 },
                new City { CityId = 7, Name = "London", StateId = 5 },
                new City { CityId = 8, Name = "Mumbai", StateId = 6 },
                new City { CityId = 9, Name = "Pune", StateId = 6 }
            );
        }

        // DbSet cho từng model
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
    }
}