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
    public class AsynchronousReturnTypesController : ControllerBase
    {
        // Danh sách nhân viên tĩnh để mô phỏng nguồn dữ liệu
        private static readonly List<EmployeeReturnType> Employees = new List<EmployeeReturnType>
        {
            new EmployeeReturnType { Id = 1, Name = "John Doe", Gender = "Male", City = "New York", Age = 30, Department = "HR" },
            new EmployeeReturnType { Id = 2, Name = "Jane Smith", Gender = "Female", City = "Los Angeles", Age = 25, Department = "Finance" },
            new EmployeeReturnType { Id = 3, Name = "Mike Johnson", Gender = "Male", City = "Chicago", Age = 40, Department = "IT" }
        };

        // Đọc (GET tất cả nhân viên)
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EmployeeReturnType>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                // Mô phỏng thao tác bất đồng bộ
                await Task.Delay(TimeSpan.FromSeconds(1));
                // Trả về danh sách nhân viên với trạng thái 200 OK
                return Ok(Employees);
            }
            catch (Exception)
            {
                // Trả về 500 Internal Server Error nếu có ngoại lệ
                return StatusCode(500, "Lỗi máy chủ nội bộ");
            }
        }

        // Đọc (GET nhân viên theo ID)
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmployeeReturnType), 200)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(typeof(object), 500)]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
                // Mô phỏng thao tác bất đồng bộ
                await Task.Delay(TimeSpan.FromSeconds(1));
                // Tìm nhân viên với ID được chỉ định
                var employee = Employees.FirstOrDefault(e => e.Id == id);
                if (employee == null)
                {
                    // Nếu không tìm thấy nhân viên, trả về trạng thái 404 Not Found với thông báo tùy chỉnh
                    return NotFound(new { message = $"Không tìm thấy nhân viên với ID {id}" });
                }
                // Nếu tìm thấy nhân viên, trả về với trạng thái 200 OK
                return Ok(employee);
            }
            catch (Exception)
            {
                // Trả về 500 Internal Server Error nếu có ngoại lệ
                return StatusCode(500, "Lỗi máy chủ nội bộ");
            }
        }

        // Tạo (POST nhân viên mới)
        [HttpPost]
        [ProducesResponseType(typeof(Employee), 201)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(object), 500)]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeReturnType employee)
        {
            try
            {
                // Mô phỏng thao tác bất đồng bộ
                await Task.Delay(TimeSpan.FromSeconds(1));
                // Xác thực dữ liệu nhân viên
                if (employee == null || string.IsNullOrEmpty(employee.Name))
                {
                    // Nếu dữ liệu không hợp lệ, trả về trạng thái 400 Bad Request với thông báo tùy chỉnh
                    return BadRequest(new { Message = "Dữ liệu nhân viên không hợp lệ" });
                }
                // Gán ID mới cho nhân viên
                employee.Id = Employees.Count + 1;
                // Thêm nhân viên vào danh sách
                Employees.Add(employee);
                // Trả về trạng thái 201 Created với tiêu đề location trỏ đến nhân viên mới tạo
                return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
            }
            catch (Exception)
            {
                // Trả về 500 Internal Server Error nếu có ngoại lệ
                return StatusCode(500, "Lỗi máy chủ nội bộ");
            }
        }

        // Cập nhật (PUT nhân viên hiện có)
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeReturnType employee)
        {
            try
            {
                // Mô phỏng thao tác bất đồng bộ
                await Task.Delay(TimeSpan.FromSeconds(1));
                // Xác thực dữ liệu nhân viên
                if (employee == null || id != employee.Id)
                {
                    // Nếu dữ liệu không hợp lệ, trả về trạng thái 400 Bad Request với thông báo tùy chỉnh
                    return BadRequest(new { Message = "Dữ liệu nhân viên không hợp lệ" });
                }
                // Tìm nhân viên hiện có với ID được chỉ định
                var existingEmployee = Employees.FirstOrDefault(e => e.Id == id);
                if (existingEmployee == null)
                {
                    // Nếu không tìm thấy nhân viên, trả về trạng thái 404 Not Found
                    return NotFound();
                }
                // Cập nhật các thuộc tính của nhân viên
                existingEmployee.Name = employee.Name;
                existingEmployee.Gender = employee.Gender;
                existingEmployee.City = employee.City;
                existingEmployee.Age = employee.Age;
                existingEmployee.Department = employee.Department;
                // Trả về trạng thái 204 No Content để chỉ ra rằng việc cập nhật thành công
                return NoContent();
            }
            catch (Exception)
            {
                // Trả về 500 Internal Server Error nếu có ngoại lệ
                return StatusCode(500);
            }
        }

        // Xóa (DELETE nhân viên)
        [HttpDelete("{id}")]
        [ProducesResponseType(200)] // Không có dữ liệu với 200 OK
        [ProducesResponseType(404)] // Không có dữ liệu với 404 Not Found
        [ProducesResponseType(500)] // Không có dữ liệu với 500 Internal Server Error
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                // Mô phỏng thao tác bất đồng bộ
                await Task.Delay(TimeSpan.FromSeconds(1));
                // Tìm nhân viên với ID được chỉ định
                var employee = Employees.FirstOrDefault(e => e.Id == id);
                if (employee == null)
                {
                    // Nếu không tìm thấy nhân viên, trả về trạng thái 404 Not Found
                    return NotFound();
                }
                // Xóa nhân viên khỏi danh sách
                Employees.Remove(employee);
                // Trả về trạng thái 200 OK không có nội dung
                return Ok();
            }
            catch (Exception)
            {
                // Trả về 500 Internal Server Error nếu có ngoại lệ
                return StatusCode(500);
            }
        }
    }
}