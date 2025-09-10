using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTAuthServer.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthServer.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor accepting DbContextOptions and passing them to the base class.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // Override OnModelCreating to configure entity properties and relationships.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure the UserRole entity as a join table for User and Role.
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId }); // Composite primary key.
            //Defines the many-to-many relationship between User and Role.
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            // Cấu hình các mối quan hệ
            // Khi một User bị xóa, các refresh token liên quan của họ cũng sẽ bị xóa để duy trì tính toàn vẹn dữ liệu.
            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Khi một Client bị xóa, các refresh token liên quan của họ cũng sẽ bị xóa để duy trì tính toàn vẹn dữ liệu.
            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.Client)
                .WithMany(c => c.RefreshTokens)
                .HasForeignKey(rt => rt.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
            // Seed initial data for Roles, Users, Clients, and UserRoles.
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin", Description = "Admin Role" },
                new Role { Id = 2, Name = "Editor", Description = " Editor Role" },
                new Role { Id = 3, Name = "User", Description = "User Role" }
            );
            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    Id = 1,
                    ClientId = "Client1",
                    Name = "Client Application 1",
                    ClientURL = "https://client1.com"
                },
                new Client
                {
                    Id = 2,
                    ClientId = "Client2",
                    Name = "Client Application 2",
                    ClientURL = "https://client2.com"
                }
            );
        }
        // DbSet representing the Users table.
        public DbSet<User> Users { get; set; }
        // DbSet representing the Roles table.
        public DbSet<Role> Roles { get; set; }
        // DbSet representing the Clients table.
        public DbSet<Client> Clients { get; set; }
        // DbSet representing the UserRoles join table.
        public DbSet<UserRole> UserRoles { get; set; }
        // DbSet đại diện cho bảng SigningKeys.
        public DbSet<SigningKey> SigningKeys { get; set; }
        // DbSet đại diện cho bảng RefreshTokens.
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        
    }
}