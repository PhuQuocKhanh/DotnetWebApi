using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModelBinding.Models;

namespace ModelBinding.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EmployeeExcludePropertiesController : ControllerBase
    {
        private static readonly List<EmployeeExcludeProperties> listEmployees = new List<EmployeeExcludeProperties>()
        {
            new (){ Id = 1, Name = "Anurag", Age = 28, Salary = 1000, Gender = "Male", Department = "IT" },
            new (){ Id = 2, Name = "Pranaya", Age = 28, Salary = 2000, Gender = "Male", Department = "IT" },
        };

        // Returns the list of all employees.
        // GET api/Employee
        [HttpGet]
        public ActionResult<List<EmployeeExcludeProperties>> GetEmployees()
        {
            return Ok(listEmployees);
        }
        // Adds a new employee to the list. 
        // POST api/Employee
        [HttpPost]
        public ActionResult<Employee> AddEmployee(EmployeeExcludeProperties employee)
        {
            if (employee != null)
            {
                // Manually setting the Id and Salary to prevent client manipulation
                employee.Id = listEmployees.Count + 1;
                employee.Salary = 3000;
                listEmployees.Add(employee);
                return Ok(employee);
            }
            return BadRequest();
        }

         [HttpGet]
        public ActionResult<List<EmployeeExcludePropertiesDTO>> GetEmployeesDto()
        {
            var employeesDTO = listEmployees.Select(emp => new EmployeeExcludePropertiesDTO
            {
                Name = emp.Name,
                Age = emp.Age,
                Gender = emp.Gender,
                Department = emp.Department
            }).ToList();
            return Ok(employeesDTO);
        }

        [HttpPost]
        public ActionResult<EmployeeExcludePropertiesDTO> AddEmployeeDto(EmployeeExcludePropertiesDTO employeeDTO)
        {
            if (employeeDTO != null)
            {
                var newEmployee = new EmployeeExcludeProperties
                {
                    Id = listEmployees.Count + 1,       // Thiết lập từ server
                    Salary = 3000,                      // Thiết lập từ server
                    Name = employeeDTO.Name,
                    Age = employeeDTO.Age,
                    Gender = employeeDTO.Gender,
                    Department = employeeDTO.Department
                };
                listEmployees.Add(newEmployee);
                return Ok(employeeDTO);
            }
            return BadRequest();
        }
    }
}