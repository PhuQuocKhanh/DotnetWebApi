using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UserServiceAPI.Models
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Seeding dummy data
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FullName = "John Doe", Email = "john.doe@example.com", Position = "Developer" },
                new User { Id = 2, FullName = "Jane Smith", Email = "jane.smith@example.com", Position = "Manager" },
                new User { Id = 3, FullName = "Sara Taylor", Email = "sara.taylor@example.com", Position = "Manager" },
                new User { Id = 4, FullName = "Pam Jordon", Email = "pam.jordon@example.com", Position = "Developer" }
            );
        }
        public DbSet<User> Users { get; set; } = null!;
    }
}