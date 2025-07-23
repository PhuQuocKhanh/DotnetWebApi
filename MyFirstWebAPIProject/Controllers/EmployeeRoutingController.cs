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
    public class EmployeeRoutingController : ControllerBase
    {
        [HttpGet("{id}")]
        public ActionResult<EmployeeRouting> GetEmployeeById(int id)
        {
            var employee = EmployeeRoutingData.Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
                return NotFound($"Employee with ID {id} not found.");
            return Ok(employee);
        }
        [HttpGet("Gender/{gender}/City/{city}")]
        public ActionResult<IEnumerable<EmployeeRouting>> GetEmployeesByGenderAndCity(string gender, string city)
        {
            var filteredEmployees = EmployeeRoutingData.Employees
                                    .Where(e => e.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase) &&
                                                e.City.Equals(city, StringComparison.OrdinalIgnoreCase))
                                    .ToList();
            if (!filteredEmployees.Any())
                return NotFound($"No employees found with Gender '{gender}' in City '{city}'.");
            return Ok(filteredEmployees);
        }

        // GET api/Employee/Search?Department=HR
        [HttpGet("Search")]
        public ActionResult<IEnumerable<EmployeeRouting>> SearchEmployees([FromQuery] string department)
        {
            var filteredEmployees = EmployeeRoutingData.Employees
                                    .Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase))
                                    .ToList();
            if (!filteredEmployees.Any())
                return NotFound($"No employees found in Department '{department}'.");
            return Ok(filteredEmployees);
        }

        // GET api/Employee/Search?Gender=Male&Department=IT&City=Los Angeles
        [HttpGet("MultiSearch")]
        public ActionResult<IEnumerable<EmployeeRouting>> SearchEmployees([FromQuery] string? gender, [FromQuery] string? department, [FromQuery] string? city)
        {
            var filteredEmployees = EmployeeRoutingData.Employees.AsQueryable();
            if (!string.IsNullOrEmpty(gender))
                filteredEmployees = filteredEmployees.Where(e => e.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(department))
                filteredEmployees = filteredEmployees.Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(city))
                filteredEmployees = filteredEmployees.Where(e => e.City.Equals(city, StringComparison.OrdinalIgnoreCase));
            var result = filteredEmployees.ToList();
            if (!result.Any())
                return NotFound("No employees match the provided search criteria.");
            return Ok(result);
        }

        // GET api/Employee/Search?Gender=Male&Department=IT&City=Los Angeles
        [HttpGet("SearchModelBinding")]
        public ActionResult<IEnumerable<EmployeeRouting>> SearchEmployees([FromQuery] EmployeeSearch searchCriteria)
        {
            var filteredEmployees = EmployeeRoutingData.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(searchCriteria.Gender))
                filteredEmployees = filteredEmployees.Where(e => e.Gender.Equals(searchCriteria.Gender, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(searchCriteria.Department))
                filteredEmployees = filteredEmployees.Where(e => e.Department.Equals(searchCriteria.Department, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(searchCriteria.City))
                filteredEmployees = filteredEmployees.Where(e => e.City.Equals(searchCriteria.City, StringComparison.OrdinalIgnoreCase));

            var result = filteredEmployees.ToList();

            if (!result.Any())
                return NotFound("No employees match the provided search criteria.");

            return Ok(result);
        }

        // GET api/Employee/DirectSearch?Gender=Male&Department=IT
        [HttpGet("DirectSearch")]
        public ActionResult<IEnumerable<EmployeeRouting>> DirectSearchEmployees()
        {
            var gender = HttpContext.Request.Query["Gender"].ToString();
            var department = HttpContext.Request.Query["Department"].ToString();
            var city = HttpContext.Request.Query["City"].ToString();

            var filteredEmployees = EmployeeRoutingData.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(gender))
                filteredEmployees = filteredEmployees.Where(e => e.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(department))
                filteredEmployees = filteredEmployees.Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(city))
                filteredEmployees = filteredEmployees.Where(e => e.City.Equals(city, StringComparison.OrdinalIgnoreCase));

            var result = filteredEmployees.ToList();

            if (!result.Any())
                return NotFound("Không tìm thấy nhân viên nào phù hợp với tiêu chí tìm kiếm.");

            return Ok(result);
        }
        
        // GET api/Employee/Gender/Male?Department=IT&City=Los Angeles
        [HttpGet("Gender/{gender}")]
        public ActionResult<IEnumerable<Employee>> GetEmployeesByGender(
            [FromRoute] string gender, 
            [FromQuery] string? department, 
            [FromQuery] string? city)
        {
            var filteredEmployees = EmployeeRoutingData.Employees
                .Where(e => e.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(department))
                filteredEmployees = filteredEmployees.Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase));
            
            if (!string.IsNullOrEmpty(city))
                filteredEmployees = filteredEmployees.Where(e => e.City.Equals(city, StringComparison.OrdinalIgnoreCase));

            var result = filteredEmployees.ToList();
            
            if (!result.Any())
                return NotFound("Không tìm thấy nhân viên nào phù hợp với tiêu chí tìm kiếm.");
            
            return Ok(result);
        }
    }
}