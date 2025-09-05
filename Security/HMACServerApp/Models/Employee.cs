using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HMACServerApp.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(50)]
        public string Position { get; set; } = null!;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }
    }
}