using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapperReverse.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoMapperReverse.Data
{
    public class EmployeeDBContext : DbContext
    {
        public EmployeeDBContext(DbContextOptions<EmployeeDBContext> options)
            : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Employee data
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, FirstName = "Anurag", LastName = "Mohanty", Email = "anurag.mohanty@example.com", Gender = "Male" },
                new Employee { Id = 2, FirstName = "Pranaya", LastName = "Rout", Email = "pranaya.rout@example.com", Gender = "Male" }
            );
            // Seed Address data linked to employees
            modelBuilder.Entity<Address>().HasData(
                new Address { Id = 1, City = "Bhubaneswar", State = "Odisha", Country = "India", EmployeeId = 1 },
                new Address { Id = 2, City = "Mumbai", State = "Maharashtra", Country = "India", EmployeeId = 2 }
            );
        }
        // Define DbSets for Employee and Address
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}