using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstWebAPIProject.Models
{
    public class EmployeeRoutingData
    {
        public static List<EmployeeRouting> Employees = new List<EmployeeRouting>
        {
            new EmployeeRouting { Id = 1, Name = "Alice Johnson", Gender = "Female", Department = "HR", City = "New York" },
            new EmployeeRouting { Id = 2, Name = "Bob Smith", Gender = "Male", Department = "IT", City = "Los Angeles" },
            new EmployeeRouting { Id = 3, Name = "Charlie Davis", Gender = "Male", Department = "Finance", City = "Chicago" },
            new EmployeeRouting { Id = 4, Name = "Sara Taylor", Gender = "Female", Department = "HR", City = "Los Angeles" },
            new EmployeeRouting { Id = 5, Name = "James Smith", Gender = "Male", Department = "IT", City = "Chicago" },
            // Add more employees as needed
        };
    }
}