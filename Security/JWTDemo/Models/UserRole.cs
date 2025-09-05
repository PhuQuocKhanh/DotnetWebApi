using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTDemo.Models
{
    public class UserRole
    {
        // Khóa ngoại tham chiếu đến User.
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // Khóa ngoại tham chiếu đến Role.
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
    }
}