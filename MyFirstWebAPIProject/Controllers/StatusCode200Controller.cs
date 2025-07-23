using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFirstWebAPIProject.Models;

namespace MyFirstWebAPIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusCode200Controller : ControllerBase
    {
        [HttpGet("GetEmployeesOK")]
        public ActionResult<List<EmployeeReturnType>> GetEmployees()
        {
            var listEmployees = new List<EmployeeReturnType>()
            {
                new (){ Id = 1001, Name = "Anurag", Age = 28, City = "Mumbai", Gender = "Male", Department = "IT" },
                new (){ Id = 1002, Name = "Pranaya", Age = 28, City = "Delhi", Gender = "Male", Department = "IT" },
                new (){ Id = 1003, Name = "Priyanka", Age = 27, City = "BBSR", Gender = "Female", Department = "HR"},
            };
            return Ok(listEmployees);
        }

        [HttpGet("GetEmployees")]
        public ActionResult<List<Employee>> GetEmployeesManualStatuscode()
        {
            var listEmployees = new List<EmployeeReturnType>()
            {
                new EmployeeReturnType(){ Id = 1001, Name = "Anurag", Age = 28, City = "Mumbai", Gender = "Male", Department = "IT" },
                new EmployeeReturnType(){ Id = 1002, Name = "Pranaya", Age = 28, City = "Delhi", Gender = "Male", Department = "IT" },
                new EmployeeReturnType(){ Id = 1003, Name = "Priyanka", Age = 27, City = "BBSR", Gender = "Female", Department = "HR"},
            };
            //return StatusCode(200); // Manually setting 200 OK without Data
            return StatusCode(200, listEmployees); // Manually setting 200 OK with Data
        }
    }
}