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
    public class EmployeeIActionReturnController : ControllerBase
    {
        [HttpGet("All")]
        //Vì chúng ta sẽ trả về Ok, StatusCode và NotFound Result từ phương thức này,
        //nên sử dụng IActionResult làm kiểu trả về
        public IActionResult GetAllEmployee()
        {
            try
            {
                //Trong thực tế, dữ liệu sẽ được lấy từ database
                //Ở đây, chúng ta hardcode dữ liệu
                var listEmployees = new List<EmployeeReturnType>()
                {
                    new EmployeeReturnType(){ Id = 1001, Name = "Anurag", Age = 28, City = "Mumbai", Gender = "Male", Department = "IT" },
                    new EmployeeReturnType(){ Id = 1002, Name = "Pranaya", Age = 28, City = "Delhi", Gender = "Male", Department = "IT" },
                    new EmployeeReturnType(){ Id = 1003, Name = "Priyanka", Age = 27, City = "BBSR", Gender = "Female", Department = "HR"},
                };

                //Nếu có ít nhất một nhân viên, trả về mã trạng thái OK và danh sách nhân viên
                if (listEmployees.Any())
                {
                    return Ok(listEmployees);
                }
                else
                {
                    //Nếu không có nhân viên nào, trả về mã trạng thái Not Found
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                // Trả về mã lỗi 500 Internal Server Error
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        //As the following method is going to return Ok, Internal Server Error and NotFound Result
        //So, we are using IActionResult as the return type of this method
        [HttpGet("{Id}")]
        public IActionResult GetEmployeeDetails(int Id)
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