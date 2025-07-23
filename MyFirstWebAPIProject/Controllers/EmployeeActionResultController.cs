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
    public class EmployeeActionResultController : ControllerBase
    {
        [HttpGet("All")]
        public ActionResult<List<EmployeeReturnType>> GetAllEmployee()
        {
            try
            {
                //In Real-Time, you will get the data from the database, here, we have hardcoded the data
                var listEmployees = new List<EmployeeReturnType>()
                {
                    new EmployeeReturnType(){ Id = 1001, Name = "Anurag", Age = 28, City = "Mumbai", Gender = "Male", Department = "IT" },
                    new EmployeeReturnType(){ Id = 1002, Name = "Pranaya", Age = 28, City = "Delhi", Gender = "Male", Department = "IT" },
                    new EmployeeReturnType(){ Id = 1003, Name = "Priyanka", Age = 27, City = "BBSR", Gender = "Female", Department = "HR"},
                };
                //If at least one Employee is Present return OK status code and the list of employees
                if (listEmployees.Any())
                {
                    return Ok(listEmployees);
                }
                else
                {
                    //If no Employee is Present return Not Found Status Code
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Log the exception 
                // Return a 500 Internal Server Error status code
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("{Id}")]
        public ActionResult<EmployeeReturnType> GetEmployeeDetails(int Id)
        {
            try
            {
                //In Real-Time, you will get the data from the database, here, we have hardcoded the data
                var listEmployees = new List<EmployeeReturnType>()
                {
                    new EmployeeReturnType(){ Id = 1001, Name = "Anurag", Age = 28, City = "Mumbai", Gender = "Male", Department = "IT" },
                    new EmployeeReturnType(){ Id = 1002, Name = "Pranaya", Age = 28, City = "Delhi", Gender = "Male", Department = "IT" },
                    new EmployeeReturnType(){ Id = 1003, Name = "Priyanka", Age = 27, City = "BBSR", Gender = "Female", Department = "HR"},
                };
                //Fetch the Employee Data based on the Employee Id
                var employee = listEmployees.FirstOrDefault(emp => emp.Id == Id);
                //If Employee Exists Return OK with the Employee Data
                if (employee != null)
                {
                    return Ok(employee);
                }
                else
                {
                    //If Employee Does Not Exists Return NotFound
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                // Return a 500 Internal Server Error status code
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}