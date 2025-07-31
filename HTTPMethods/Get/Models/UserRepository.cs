using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTPMethods.Get.Models
{
    public class UserRepository
    {
        public List<UserGet> Users = new List<UserGet>()
        {
            new () { Id = 1, Name= "Pranaya", Email = "Pranaya@Example.com" },
            new () { Id = 2, Name= "Anurag", Email = "Anurag@Example.com" },
            new () { Id = 3, Name= "Priyanka", Email = " Priyanka@Example.com" }
        };

        public List<OrderGet> Orders = new List<OrderGet>()
        {
            new () { Id = 1001, UserId = 1, TotalAmount = 100 },
            new () { Id = 1002, UserId = 2, TotalAmount = 200 },
            new () { Id = 1003, UserId = 1, TotalAmount = 300 },
            new () { Id = 1004, UserId = 2, TotalAmount = 400 },
            new () { Id = 1005, UserId = 3, TotalAmount = 500 }
        };

        // Lấy danh sách tất cả người dùng
        public IEnumerable<UserGet> GetAll()
        {
            var users = Users.ToList();
            foreach (var user in users)
            {
                user.Orders = [.. Orders.Where(ord => ord.UserId == user.Id)];
            }
            return users;
        }

        // Lấy thông tin người dùng theo ID
        public UserGet GetById(int Id)
        {
            var user = Users.FirstOrDefault(u => u.Id == Id);
            if (user != null)
            {
                user.Orders = Orders.Where(ord => ord.UserId == user.Id).ToList();
            }
            return user;
        }

        // Tìm kiếm người dùng theo tên
        public IEnumerable<UserGet> SearchByName(string name)
        {
            var users = Users.Where(u => u.Name.Contains(name)).ToList();
            foreach (var user in users)
            {
                user.Orders = Orders.Where(ord => ord.UserId == user.Id).ToList();
            }
            return users;
        }

        // Lấy danh sách đơn hàng theo UserId
        public IEnumerable<OrderGet> GetOrderByUserId(int UserId)
        {
            var userWithOrders = Orders.Where(ord => ord.UserId == UserId);
            return userWithOrders ?? new List<OrderGet>();
        }
    }
}