using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AESServerAPP.Models;
using Microsoft.EntityFrameworkCore;

namespace AESServerAPP.Controllers
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình ràng buộc duy nhất trên ClientId
            modelBuilder.Entity<ClientKeyIV>()
                .HasIndex(c => c.ClientId)
                .IsUnique();
            
            // Seed dữ liệu ban đầu cho ClientKeyIV
            modelBuilder.Entity<ClientKeyIV>().HasData(
                new ClientKeyIV { Id = 1, ClientId = "DefaultClient", Key = "Yyj9nVLtBLwPANTqZNFHrofcH/AbvJlaUbytoHT8Qd8=", IV = "/X9EAc4vBALd31ye7N3L1g==" }
                //... các client khác
            );

            // Seed dữ liệu ban đầu cho Employee
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, Name = "Alice Smith", Salary = 75000m }
                //... các nhân viên khác
            );
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<ClientKeyIV> ClientKeyIVs { get; set; }
    }
}