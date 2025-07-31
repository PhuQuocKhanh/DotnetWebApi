using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapperReverse.Data;
using AutoMapperReverse.DTOs;
using AutoMapperReverse.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoMapperReverse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly EmployeeDBContext _context;

        public EmployeesController(IMapper mapper, EmployeeDBContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("GetEmployees")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees()
        {
            var employees = await _context.Employees
                .AsNoTracking()
                .Include(e => e.Address)
                .ToListAsync();

            var employeeDTOs = _mapper.Map<List<EmployeeDTO>>(employees);
            return Ok(employeeDTOs);
        }

        [HttpGet("GetEmployee/{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .Include(e => e.Address)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return NotFound($"Employee with ID {id} not found.");

            var employeeDTO = _mapper.Map<EmployeeDTO>(employee);
            return Ok(employeeDTO);
        }

        [HttpPost("AddEmployee")]
        public async Task<ActionResult<EmployeeDTO>> AddEmployee(EmployeeDTO employeeDTO)
        {
            if (employeeDTO == null)
                return BadRequest("Employee data is null.");

            var employee = _mapper.Map<Employee>(employeeDTO);
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            var createdEmployeeDTO = _mapper.Map<EmployeeDTO>(employee);
            return Ok(createdEmployeeDTO);
        }

        [HttpPut("UpdateEmployee/{id}")]
        public async Task<ActionResult<EmployeeDTO>> UpdateEmployee(int id, EmployeeDTO employeeDTO)
        {
            if (id != employeeDTO.EmployeeId)
                return BadRequest("Employee ID mismatch.");

            var existingEmployee = await _context.Employees
                .Include(e => e.Address)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (existingEmployee == null)
                return NotFound($"Employee with ID {id} not found.");

            _mapper.Map(employeeDTO, existingEmployee);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!EmployeeExists(id))
                    return NotFound($"Employee with ID {id} no longer exists.");
                else
                    return StatusCode(500, $"An error occurred: {ex.Message}");
            }

            var updatedEmployeeDTO = _mapper.Map<EmployeeDTO>(existingEmployee);
            return Ok(updatedEmployeeDTO);
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}