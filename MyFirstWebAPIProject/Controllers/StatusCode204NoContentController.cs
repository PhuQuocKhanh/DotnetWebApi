using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFirstWebAPIProject.Models;

namespace MyFirstWebAPIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StatusCode204NoContentController : ControllerBase
    {
        //Data Source
        private static List<EmployeeReturnType> Employees = new List<EmployeeReturnType>
        {
            new() { Id = 1, Name = "Rakesh", Age = 25, Gender = "Male", Department = "IT" },
            new() { Id = 2, Name = "Hina", Age = 26, Gender = "Female", Department = "HR" },
            new() { Id = 3, Name = "Suresh", Age = 27, Gender = "Male", Department = "IT" },
        };
        //An example PUT method that uses the 204 No Content response
        //PUT: api/Employee/5
        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, EmployeeReturnType employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }
            var existingEmployee = Employees.FirstOrDefault(emp => emp.Id == id);
            if (existingEmployee == null)
            {
                return NotFound();
            }
            existingEmployee.Name = employee.Name;
            existingEmployee.Age = employee.Age;
            existingEmployee.Gender = employee.Gender;
            existingEmployee.Department = employee.Department;
            // In a real application, here you would update the product in the database
            // If the employee is successfully updated, return a 204 No Content response.
            // This indicates to the client that the request was successful but there is no content to send back.
            return NoContent();
        }
        //An example DELETE method that uses the 204 No Content response
        //DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var existingEmployee = Employees.FirstOrDefault(emp => emp.Id == id);
            if (existingEmployee == null)
            {
                return NotFound();
            }
            Employees.Remove(existingEmployee);
            // In a real application, here you would delete the product from the database
            // If the employee is successfully deleted, return a 204 No Content response.
            // This indicates to the client that the request was successful but there is no content to send back.
            return NoContent();
        }
    }
}