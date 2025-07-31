using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutomapperNullSubstitute.DTOs;
using AutomapperNullSubstitute.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutomapperNullSubstitute.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeComplexTypeController : ControllerBase
    {
        private readonly IMapper _mapper;

        // Dữ liệu mẫu
        public List<EmployeeComplexType> listEmployees = new List<EmployeeComplexType>()
        {
            new EmployeeComplexType(){Id = 1, Name="Pranaya", Department="IT", Address = null, CreatedBy=null, CreatedOn=null },
            new EmployeeComplexType(){Id = 2, Name="Anurag", Department="HR", Address = new AddressComplexType(){City="BBSR", State="Odisha", Country="India"}, CreatedBy=null, CreatedOn=DateTime.Now },
            new EmployeeComplexType(){Id = 3, Name="Priyanla", Department="HR", Address = null, CreatedBy="Admin", CreatedOn=null }
        };

        public EmployeeComplexTypeController(IMapper mapper)
        {
            _mapper = mapper;
        }

        // API: GET api/Employee/1
        [HttpGet("{Id}")]
        public ActionResult<EmployeeDTOComplexType> GetEmployee(int Id)
        {
            var employee = listEmployees.FirstOrDefault(emp => emp.Id == Id);
            if (employee == null)
                return NotFound("Employee Not Found");

            var employeeDTO = _mapper.Map<EmployeeDTOComplexType>(employee);
            return Ok(employeeDTO);
        }
    }
}