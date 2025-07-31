using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTTPMethods.Get.Models;
using Microsoft.AspNetCore.Mvc;

namespace HTTPMethods.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserGetController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserGetController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Endpoint: Lấy danh sách tất cả người dùng
        // GET api/users
        [HttpGet]
        public ActionResult<List<UserGet>> GetAllUsers()
        {
            var users = _userRepository.GetAll();
            return Ok(users);
        }

        // Endpoint: Lấy thông tin người dùng theo Id
        // GET api/users/1
        [HttpGet]
        [Route("api/users/{Id}")]
        public ActionResult<UserGet> GetUser(int Id)
        {
            var user = _userRepository.GetById(Id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // Endpoint: Tìm kiếm người dùng theo tên (qua query string)
        // GET api/users/search?name=pranaya
        [HttpGet]
        [Route("search")]
        public ActionResult<List<UserGet>> SearchUsers(string name)
        {
            var users = _userRepository.SearchByName(name);
            if(users == null || !users.Any())
            {
                return NotFound();
            }
            return Ok(users);
        }

        // Endpoint: Lấy danh sách đơn hàng của một người dùng cụ thể
        // GET api/users/1/orders
        [HttpGet]
        [Route("api/users/{userId}/orders")]
        public ActionResult<List<OrderGet>> GetUserOrders(int userId)
        {
            var orders = _userRepository.GetOrderByUserId(userId);
            if (orders == null || !orders.Any())
            {
                return NotFound();
            }
            return Ok(orders);
        }
    }
}