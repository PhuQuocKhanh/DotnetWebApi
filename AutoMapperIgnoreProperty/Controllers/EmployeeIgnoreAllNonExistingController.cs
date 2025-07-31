using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapperIgnoreProperty.DTOs;
using AutoMapperIgnoreProperty.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoMapperIgnoreProperty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeIgnoreAllNonExistingController : ControllerBase
    {
        private readonly IMapper _mapper;

        public List<EmployeeIgnoreAllNonExisting> listEmployees = new List<EmployeeIgnoreAllNonExisting>()
        {
            new (){ Id = 1, Name = "Pranaya", Department = "IT", Position = "DBA", Salary = 1000, 
                City = "BBSR", State = "Odisha", Country = "India" },
            new (){ Id = 2, Name = "Anurag", Department = "HR", Position = "Developer", Salary = 2000,
                City = "CTC", State = "Odisha", Country = "India" }
        };

        public EmployeeIgnoreAllNonExistingController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("{Id}")]
        public ActionResult<EmployeeDTOIgnoreAllNonExisting> GetEmployee(int Id)
        {
            var employee = listEmployees.FirstOrDefault(emp => emp.Id == Id);
            if (employee == null)
            {
                return NotFound("Không tìm thấy nhân viên");
            }

            var employeeDTO = _mapper.Map<EmployeeDTOIgnoreAllNonExisting>(employee);
            return Ok(employeeDTO);
        }
    }
}