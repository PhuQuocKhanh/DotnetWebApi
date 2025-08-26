using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAPIConditionValidator.Data;
using FluentAPIConditionValidator.DTOs;
using FluentAPIConditionValidator.Models;
using FluentAPIConditionValidator.Validators;
using Microsoft.AspNetCore.Mvc;

namespace FluentAPIConditionValidator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ECommerceDbContext _context;
        public OrdersController(ECommerceDbContext context)
        {
            _context = context;
        }
        // Tạo một đơn hàng mới dựa trên OrderDTO được cung cấp.
        // Áp dụng validations có điều kiện và trả về lỗi nếu có.
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDTO)
        {
            // Khởi tạo validator với DbContext hiện tại cho bất kỳ kiểm tra nào liên quan đến cơ sở dữ liệu.
            var validator = new OrderDTOValidator(_context);
            var validationResult = await validator.ValidateAsync(orderDTO);

            // Nếu validation thất bại, trả về 400 Bad Request với chi tiết lỗi.
            if (!validationResult.IsValid)
            {
                var errorResponse = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,  // Thuộc tính không hợp lệ
                    Error = e.ErrorMessage    // Thông báo lỗi chi tiết
                });
                return BadRequest(new { Errors = errorResponse });
            }

            // Ánh xạ DTO đã được xác thực sang thực thể Order.
            var order = new Order
            {
                CustomerId = orderDTO.CustomerId,
                PaymentMode = orderDTO.PaymentMode,
                CreditCardNumber = orderDTO.CreditCardNumber,
                UPIId = orderDTO.UPIId,
                OrderAmount = orderDTO.OrderAmount,
                Discount = orderDTO.Discount,
                OrderDate = DateTime.Now,
                ShippingAddress = orderDTO.ShippingAddress
            };

            // Lưu đơn hàng vào cơ sở dữ liệu một cách bất đồng bộ.
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Order created successfully.", OrderId = order.OrderId });
        }
    }
}