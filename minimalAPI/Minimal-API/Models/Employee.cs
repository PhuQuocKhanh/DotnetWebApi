using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Minimal_API.Models
{
    public class Employee
    {
        // Id sinh tự động, không cần validation
        public int Id { get; set; }

        // Name bắt buộc, tối đa 100 ký tự
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = null!;

        // Position bắt buộc, tối đa 50 ký tự
        [Required(ErrorMessage = "Position is required")]
        [StringLength(50, ErrorMessage = "Position cannot exceed 50 characters")]
        public string Position { get; set; } = null!;

        // Salary bắt buộc, trong khoảng 30,000 – 200,000
        [Required(ErrorMessage = "Salary is required")]
        [Range(30000, 200000, ErrorMessage = "Salary must be between 30,000 and 200,000")]
        public decimal Salary { get; set; }
    }
}