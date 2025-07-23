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
    public class EmployeeMultiUrlController : ControllerBase
    {
        // Phương thức Action với Nhiều Route
        [HttpGet("All")]
        [HttpGet("AllEmployees")]
        [HttpGet("GetAll")]
        public ActionResult<IEnumerable<EmployeeMultiUrls>> GetAllEmployees()
        {
            var employees = EmployeeMultiUrlData.Employees;
            return Ok(employees);
        }

        // Nhiều thuộc tính route để hỗ trợ cả tên cũ và mới
        [Route("api/old-employees")]
        [Route("api/staff")]
        [HttpGet]
        public ActionResult<IEnumerable<EmployeeMultiUrls>> GetAllEmployeesRealtime()
        {
            var employees = EmployeeMultiUrlData.Employees;
            return Ok(employees);
        }
    }
}