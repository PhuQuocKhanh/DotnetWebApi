using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstWebAPIProject.Models
{
    public class EmployeeMultiUrls
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Department { get; set; }
        public string City { get; set; }
    }
    public static class EmployeeMultiUrlData
    {
        public static List<EmployeeMultiUrls> Employees = new List<EmployeeMultiUrls>
        {
            new() { Id = 1, Name = "Alice Johnson", Gender = "Female", Department = "HR", City = "New York" },
            new() { Id = 2, Name = "Bob Smith", Gender = "Male", Department = "IT", City = "Los Angeles" },
            new() { Id = 3, Name = "Charlie Davis", Gender = "Male", Department = "Finance", City = "Chicago" }
        };
    }
}