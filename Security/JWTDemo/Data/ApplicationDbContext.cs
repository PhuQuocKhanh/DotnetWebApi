using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTDemo.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor chấp nhận DbContextOptions để cấu hình, được truyền cho DbContext cơ sở.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // Cấu hình các mối quan hệ thực thể, khóa và dữ liệu khởi tạo (seed data).
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Định nghĩa khóa chính phức hợp cho thực thể UserRole (bảng nối cho quan hệ nhiều-nhiều).
            // Điều này có nghĩa là sự kết hợp của UserId + RoleId sẽ định danh duy nhất một bản ghi UserRole.
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            // Cấu hình mối quan hệ: UserRole -> User (nhiều UserRole thuộc về một User).
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles) // User có thể có nhiều UserRoles.
                .HasForeignKey(ur => ur.UserId); // Khóa ngoại trong UserRole trỏ đến User.Id.

            // Cấu hình mối quan hệ: UserRole -> Role (nhiều UserRole thuộc về một Role).
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles) // Role có thể có nhiều UserRoles.
                .HasForeignKey(ur => ur.RoleId); // Khóa ngoại trong UserRole trỏ đến Role.Id.

            // Khởi tạo dữ liệu Role ban đầu trong cơ sở dữ liệu.
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "User", Description = "Vai trò người dùng thông thường" },
                new Role { Id = 2, Name = "Admin", Description = "Vai trò quản trị viên" },
                new Role { Id = 3, Name = "Editor", Description = "Vai trò biên tập viên" }
            );

            // Khởi tạo dữ liệu Client ban đầu cho việc xác thực (dành cho mục đích demo).
            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    Id = 1,
                    ClientId = "client-app-one", // Định danh client duy nhất được sử dụng trong token JWT.
                    Name = "Demo Client Application One",
                    ClientSecret = "fPXxcJw8TW5sA+S4rl4tIPcKk+oXAqoRBo+1s2yjUS4=", // Khóa bí mật được mã hóa Base64.
                    ClientURL = "https://clientappone.example.com", // Được sử dụng làm Audience trong việc xác thực JWT.
                    IsActive = true // Cờ đánh dấu client đang hoạt động.
                },
                new Client
                {
                    Id = 2,
                    ClientId = "client-app-two",
                    Name = "Demo Client Application Two",
                    ClientSecret = "UkY2JEdtWqKFY5cEUuWqKZut2o6BI5cf3oexOlCMZvQ=",
                    ClientURL = "https://clientapptwo.example.com",
                    IsActive = true
                }
            );
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    }
}