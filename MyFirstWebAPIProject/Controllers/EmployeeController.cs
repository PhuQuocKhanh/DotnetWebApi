using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFirstWebAPIProject.Models;
using MyFirstWebAPIProject.Repositories;

namespace MyFirstWebAPIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Base route: api/employee
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;
        
        public EmployeeController(IEmployeeRepository repository)
        {
            _repository = repository;
        }
        
        // GET api/employee
        [HttpGet]
        public ActionResult<IEnumerable<Employee>> GetAllEmployees()
        {
            return Ok(_repository.GetAll());
        }
        
        // GET api/employee/{id}
        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployeeById(int id) 
        {
            var employee = _repository.GetById(id);
            return employee == null ? NotFound() : Ok(employee);
        }
        
        // POST api/employee
        [HttpPost]
        public ActionResult<Employee> CreateEmployee([FromBody] Employee employee)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            _repository.Add(employee);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
        }
        
        // PUT api/employee/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, [FromBody] Employee employee)
        {
            if (id != employee.Id) return BadRequest("ID mismatch");
            if (!_repository.Exists(id)) return NotFound();
            
            _repository.Update(employee);
            return NoContent();
        }
        
        // PATCH api/employee/{id}
        [HttpPatch("{id}")]
        public IActionResult PatchEmployee(int id, [FromBody] Employee employee)
        {
            var existing = _repository.GetById(id);
            if (existing == null) return NotFound();
            
            existing.Name = employee.Name ?? existing.Name;
            existing.Position = employee.Position ?? existing.Position;
            existing.Age = employee.Age != 0 ? employee.Age : existing.Age;
            existing.Email = employee.Email ?? existing.Email;
            
            _repository.Update(existing);
            return NoContent();
        }
        
        // DELETE api/employee/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            if (!_repository.Exists(id)) return NotFound();
            
            _repository.Delete(id);
            return NoContent();
        }
    }
}