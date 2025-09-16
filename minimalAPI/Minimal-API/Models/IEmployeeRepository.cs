using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minimal_API.Models
{
    public interface IEmployeeRepository
    {
        // Lấy toàn bộ danh sách nhân viên
        List<Employee> GetAllEmployees();
        // Lấy thông tin một nhân viên theo ID, hoặc null nếu không tồn tại
        Employee? GetEmployeeById(int id);
        // Thêm một nhân viên mới và trả về nhân viên đã được gán ID
        Employee AddEmployee(Employee employee);
        // Cập nhật nhân viên theo ID, trả về nhân viên đã cập nhật hoặc null nếu không tồn tại
        Employee? UpdateEmployee(int id, Employee updatedEmployee);
        // Xóa nhân viên theo ID, trả về true nếu xóa thành công, false nếu thất bại
        bool DeleteEmployee(int id);
    }

    public class EmployeeRepository : IEmployeeRepository
    {
        // Danh sách nội bộ giả lập kho dữ liệu
        private readonly List<Employee> _employeeList;
        public EmployeeRepository()
        {
            // Khởi tạo với dữ liệu mẫu ban đầu
            _employeeList = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Position = "Software Engineer", Salary = 60000 },
                new Employee { Id = 2, Name = "Jane Smith", Position = "Project Manager", Salary = 80000 }
            };
        }
        public List<Employee> GetAllEmployees() => _employeeList;
        public Employee? GetEmployeeById(int id) => _employeeList.FirstOrDefault(e => e.Id == id);
        public Employee AddEmployee(Employee newEmployee)
        {
            newEmployee.Id = _employeeList.Count > 0 ? _employeeList.Max(emp => emp.Id) + 1 : 1;
            _employeeList.Add(newEmployee);
            return newEmployee;
        }
        public Employee? UpdateEmployee(int id, Employee updatedEmployee)
        {
            var employee = _employeeList.FirstOrDefault(emp => emp.Id == id);
            if (employee == null) return null;
            employee.Name = updatedEmployee.Name;
            employee.Position = updatedEmployee.Position;
            employee.Salary = updatedEmployee.Salary;
            return employee;
        }
        public bool DeleteEmployee(int id)
        {
            var employee = _employeeList.FirstOrDefault(emp => emp.Id == id);
            if (employee == null) return false;
            _employeeList.Remove(employee);
            return true;
        }
    }
}