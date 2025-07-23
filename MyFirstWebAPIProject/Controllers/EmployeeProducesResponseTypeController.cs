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
    public class EmployeeProducesResponseTypeController : ControllerBase
    {
        // Static list of employees to simulate a data source
        private static readonly List<EmployeeReturnType> Employees = new List<EmployeeReturnType>
        {
            new EmployeeReturnType { Id = 1, Name = "John Doe", Gender = "Male", City = "New York", Age = 30, Department = "HR" },
            new EmployeeReturnType { Id = 2, Name = "Jane Smith", Gender = "Female", City = "Los Angeles", Age = 25, Department = "Finance" },
            new EmployeeReturnType { Id = 3, Name = "Mike Johnson", Gender = "Male", City = "Chicago", Age = 40, Department = "IT" }
        };
        [HttpGet("all-OkObjectResult")]
        public OkObjectResult GetAllEmployees()
        {
            // Return the list of employees with a 200 OK status
            // OkObjectResult with data
            return Ok(Employees);
        }

        // Read (GET employee by ID)
        // This action returns a single employee based on the provided ID
        [HttpGet("ActionResult/{id}")]
        public ActionResult<Employee> GetEmployeeById(int id)
        {
            // Find the employee with the specified ID
            var employee = Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                // If the employee is not found, return a 404 Not Found status with a custom message
                // NotFoundObjectResult with additional info
                return NotFound(new { message = $"No employee found with ID {id}" });
            }
            // If the employee is found, return it with a 200 OK status
            // OkObjectResult with the employee
            return Ok(employee);
        }
        [HttpGet("All")]
        // Vì action này có thể trả về Ok, NotFound hoặc InternalServerError,
        // nên sử dụng IActionResult làm kiểu trả về
        // 200 OK: Trả về danh sách Employee
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EmployeeReturnType>))]
        // 404 Not Found: Không tìm thấy nhân viên nào
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // 500 Internal Server Error: Lỗi server không mong muốn
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllEmployee()
        {
            try
            {
                // Giả lập dữ liệu từ database
                var listEmployees = new List<EmployeeReturnType>()
                {
                    new EmployeeReturnType(){ Id = 1001, Name = "Anurag", Age = 28, City = "Mumbai", Gender = "Male", Department = "IT" },
                    new EmployeeReturnType(){ Id = 1002, Name = "Pranaya", Age = 28, City = "Delhi", Gender = "Male", Department = "IT" },
                    new EmployeeReturnType(){ Id = 1003, Name = "Priyanka", Age = 27, City = "BBSR", Gender = "Female", Department = "HR"},
                };

                if (listEmployees.Any())
                {
                    return Ok(listEmployees); // 200 OK + dữ liệu
                }
                else
                {
                    return NotFound(); // 404 Not Found
                }
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                return StatusCode(500, "An error occurred while processing your request."); // 500 Internal Server Error
            }
        }

        [HttpGet("{Id}")]
        // 200 OK: Trả về thông tin Employee
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeReturnType))]
        // 404 Not Found: Không tìm thấy Employee với Id tương ứng
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // 500 Internal Server Error: Lỗi server không mong muốn
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetEmployeeDetails(int Id)
        {
            try
            {
                var listEmployees = new List<EmployeeReturnType>()
                {
                    new EmployeeReturnType(){ Id = 1001, Name = "Anurag", Age = 28, City = "Mumbai", Gender = "Male", Department = "IT" },
                    new EmployeeReturnType(){ Id = 1002, Name = "Pranaya", Age = 28, City = "Delhi", Gender = "Male", Department = "IT" },
                    new EmployeeReturnType(){ Id = 1003, Name = "Priyanka", Age = 27, City = "BBSR", Gender = "Female", Department = "HR"},
                };

                var employee = listEmployees.FirstOrDefault(emp => emp.Id == Id);
                if (employee != null)
                {
                    return Ok(employee); // 200 OK + dữ liệu
                }
                else
                {
                    return NotFound(); // 404 Not Found
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request."); // 500 Internal Server Error
            }
        }
    }
}