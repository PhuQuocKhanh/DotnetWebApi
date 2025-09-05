using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JWTDemo.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        // Name of the role (e.g., Admin, User).
        [Required(ErrorMessage = "Role name is required.")]
        [MaxLength(20, ErrorMessage = "Role name cannot exceed 20 characters.")]
        public string Name { get; set; } = null!;
        //Role Description
        public string? Description { get; set; }
        // Navigation property for users assigned this role
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}