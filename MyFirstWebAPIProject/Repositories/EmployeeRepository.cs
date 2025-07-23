using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstWebAPIProject.Models;

namespace MyFirstWebAPIProject.Repositories
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll();
        Employee? GetById(int id);
        void Add(Employee employee);
        void Update(Employee employee);
        void Delete(int id);
        bool Exists(int id);
    }
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> _employees;
        public EmployeeRepository()
        {
            // Initialize with some sample data
            _employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Position = "Software Engineer", Age = 30, Email = "john.doe@example.com" },
                new Employee { Id = 2, Name = "Jane Smith", Position = "Project Manager", Age = 35, Email = "jane.smith@example.com" }
            };
        }
        public IEnumerable<Employee> GetAll()
        {
            return _employees;
        }
        public Employee? GetById(int id)
        {
            return _employees.FirstOrDefault(e => e.Id == id);
        }
        public void Add(Employee employee)
        {
            employee.Id = _employees.Max(e => e.Id) + 1;
            _employees.Add(employee);
        }
        public void Update(Employee employee)
        {
            var existingEmployee = GetById(employee.Id);
            if (existingEmployee != null)
            {
                existingEmployee.Name = employee.Name;
                existingEmployee.Position = employee.Position;
                existingEmployee.Age = employee.Age;
                existingEmployee.Email = employee.Email;
            }
        }
        public void Delete(int id)
        {
            var employee = GetById(id);
            if (employee != null)
            {
                _employees.Remove(employee);
            }
        }
        public bool Exists(int id)
        {
            return _employees.Any(e => e.Id == id);
        }
    }
}