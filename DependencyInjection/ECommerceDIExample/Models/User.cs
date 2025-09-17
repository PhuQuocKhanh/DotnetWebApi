using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ECommerceDIExample.Models
{
    [Index(nameof(Email), Name = "IX_Email_Unique", IsUnique =true)]
    public class User
    {
        // Unique Identifier
        public int Id { get; set; }
        // Common user details
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string Name { get; set; }
        [StringLength(20, ErrorMessage = "UserType cannot exceed 20 characters.")]
        public string? UserType { get; set; } // VIP, Premium, Standard, Guest
        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        // Soft delete indicator (true => user is active)
        public bool IsActive { get; set; } = true;
        // Navigation property: A user can have many carts 
        public ICollection<Cart>? Carts { get; set; }
    }
}