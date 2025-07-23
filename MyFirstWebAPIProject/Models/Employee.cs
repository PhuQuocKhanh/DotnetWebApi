using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstWebAPIProject.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Position { get; set; }

        [Range(18, 65)]
        public int Age { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }

    public class EmployeeRouting
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Department { get; set; }
        public string City { get; set; }
    }
}