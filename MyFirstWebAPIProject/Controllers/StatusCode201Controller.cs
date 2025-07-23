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
    public class StatusCode201Controller : ControllerBase
    {
        // Nguồn dữ liệu
        private static List<EmployeeReturnType> Employees = new List<EmployeeReturnType>
        {
            new() { Id = 1, Name = "Rakesh", Age = 25, Gender = "Male", Department = "IT" },
            new() { Id = 2, Name = "Hina", Age = 26, Gender = "Female", Department = "HR" },
            new() { Id = 3, Name = "Suresh", Age = 27, Gender = "Male", Department = "IT" },
        };

        [HttpPost]
        public ActionResult<EmployeeReturnType> CreateEmployee(EmployeeReturnType employee)
        {
            // Thêm và lưu nhân viên vào database
            employee.Id = Employees.Count + 1;
            Employees.Add(employee);

            // Trả về 201 Created với Location header
            // GetEmployee là tên action method để lấy thông tin nhân viên
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }

        [HttpPost]
        public ActionResult<EmployeeReturnType> CreateEmployeeCreateAtRoute(EmployeeReturnType employee)
        {
            // Thêm và lưu nhân viên vào database hoặc bộ nhớ
            employee.Id = Employees.Count + 1;
            Employees.Add(employee);

            // Sử dụng route có tên để lấy tài nguyên
            // GetGetEmployeeRoute là tên Route của Resource nhận id và trả về nhân viên
            return CreatedAtRoute("GetGetEmployeeRoute", new { id = employee.Id }, employee);
        }

        [HttpGet("{id}")]
        public ActionResult<EmployeeReturnType> GetEmployee(int id)
        {
            var employee = Employees.FirstOrDefault(emp => emp.Id == id);
            if (employee == null)
            {
                // 404 Not Found nếu nhân viên không tồn tại
                return NotFound();
            }
            // 200 OK với thông tin nhân viên
            return Ok(employee);
        }
        
        // POST: api/Employee/CreateEmployee1
        [HttpPost]
        public ActionResult<EmployeeReturnType> CreateEmployee1(EmployeeReturnType employee)
        {
            // Add and save employee in your database or storage
            employee.Id = Employees.Count + 1;
            Employees.Add(employee);
            // Returning 204 Status Code without Location Header and without Response Body
            return Created();
        }
        // POST: api/Employee/CreateEmployee2
        [HttpPost]
        public ActionResult<EmployeeReturnType> CreateEmployee2(EmployeeReturnType employee)
        {
            // Add and save employee in your database or storage
            employee.Id = Employees.Count + 1;
            Employees.Add(employee);
            // Create the URI for the newly created resource as a string
            // var locationUri = $"https://localhost:7166/api/Employee/GetEmployee/{employee?.Id}";
            // Generate the URI for the newly created resource using Url.Action
            // The Url.Action method generates a fully qualified URL for the specified action.
            // In this case, it generates the URL for the GetEmployee action, using the id of the newly created employee.
            var locationUri = Url.Action("GetEmployee", new { id = employee.Id });
            // Return 201 Created response with the URI and the employee object
            return Created(locationUri, employee);
        }
        // POST: api/Employee/CreateEmployee3
        [HttpPost]
        public ActionResult<EmployeeReturnType> CreateEmployee3(EmployeeReturnType employee)
        {
            // Add and save employee in your database or storage
            employee.Id = Employees.Count + 1;
            Employees.Add(employee);
            // Generate the URI for the newly created resource using Url.Link
            // The Url.Link method generates an absolute URI based on the specified route name ("GetEmployee")
            // and route values (new { id = employee.Id }).
            var locationUri = Url.Link("GetEmployee", new { id = employee.Id });
            if (locationUri == null)
            {
                // Handle the error or generate a default URI
                return BadRequest("Unable to generate location URI for the new resource.");
            }
            // Return 201 Created response with the URI and the employee object
            return Created(new Uri(locationUri), employee);
        }
        // GET: api/Employee/GetEmployee/1
        [HttpGet("Id:{id}", Name = "GetEmployeebyId")]
        public ActionResult<EmployeeReturnType> GetEmployeeById(int id)
        {
            var employee = Employees.FirstOrDefault(emp => emp.Id == id);
            if (employee == null)
            {
                // 404 Not Found if the employee does not exist
                return NotFound();
            }
            // 200 OK with the employee in the response body
            return Ok(employee);
        }
    }
}