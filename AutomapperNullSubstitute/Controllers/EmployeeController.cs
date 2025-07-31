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
    public class EmployeeController : ControllerBase
    {
        private readonly IMapper _mapper;

        public List<Employee> listEmployees = new List<Employee>()
        {
            new (){Id = 1, Name="Pranaya", Department="IT", Address = "BBSR", CreatedBy=null, CreatedOn=null },
            new (){Id = 2, Name="Anurag", Department="HR", Address = null, CreatedBy=null, CreatedOn=DateTime.Now },
            new (){Id = 3, Name="Priyanla", Department="HR", Address = null, CreatedBy="Admin", CreatedOn=null  }
        };

        public EmployeeController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("{Id}")]
        public ActionResult<EmployeeDTO> GetEmployee(int Id)
        {
            var employee = listEmployees.FirstOrDefault(emp => emp.Id == Id);
            if (employee == null)
                return NotFound("Employee Not Found");

            var employeeDTO = _mapper.Map<EmployeeDTO>(employee);
            return Ok(employeeDTO);
        }
    }
}