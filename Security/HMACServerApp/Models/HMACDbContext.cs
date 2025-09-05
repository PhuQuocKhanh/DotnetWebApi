using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HMACServerApp.Models
{
    public class HMACDbContext : DbContext
    {
        public HMACDbContext(DbContextOptions<HMACDbContext> options)
            : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ClientSecret> ClientSecrets { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Seed dữ liệu client secrets ban đầu với ClientId và SecretKey
            modelBuilder.Entity<ClientSecret>().HasData(
                new ClientSecret { Id = 1, ClientId = "WebAppClient", SecretKey = "a1b2c3d4e5f6g7h8i9j0" },
                new ClientSecret { Id = 2, ClientId = "MobileAppClient", SecretKey = "z9y8x7w6v5u4t3s2r1q0" },
                new ClientSecret { Id = 3, ClientId = "DesktopClient", SecretKey = "m1n2b3v4c5x6z7l8k9j0" }
            );
            // Seed dữ liệu nhân viên ban đầu
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, Name = "John Doe", Position = "Software Engineer", Salary = 80000m },
                new Employee { Id = 2, Name = "Jane Smith", Position = "Project Manager", Salary = 95000m }
            );
        }
    }
}