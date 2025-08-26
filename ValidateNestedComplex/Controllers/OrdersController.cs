using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ValidateNestedComplex.Data;
using ValidateNestedComplex.DTOs;
using ValidateNestedComplex.Models;
using ValidateNestedComplex.Validator;

namespace ValidateNestedComplex.Controllers
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
        // Phương thức HTTP POST để tạo một đơn hàng mới.
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDTO)
        {
            // Tạo một instance của OrderDTOValidator và truyền DbContext vào.
            var validator = new OrderDTOValidator(_context);
            // Xác thực OrderDTO đầu vào một cách bất đồng bộ.
            var validationResult = await validator.ValidateAsync(orderDTO);
            // Kiểm tra nếu xác thực thất bại.
            if (!validationResult.IsValid)
            {
                // Chiếu các lỗi xác thực thành một định dạng đơn giản với tên trường và thông báo lỗi.
                var errorResponse = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,   // Tên thuộc tính không hợp lệ.
                    Error = e.ErrorMessage    // Thông báo lỗi tương ứng.
                });
                // Trả về 400 Bad Request với danh sách các lỗi xác thực.
                return BadRequest(new { Errors = errorResponse });
            }
            int shippingAddressId;  // Biến để lưu ShippingAddressId đã được giải quyết.
            // Kiểm tra xem ShippingAddressId hợp lệ có được cung cấp trong OrderDTO không.
            if (orderDTO.ShippingAddressId.HasValue && orderDTO.ShippingAddressId.Value > 0)
            {
                // Nếu được cung cấp và hợp lệ, sử dụng ShippingAddressId từ DTO.
                shippingAddressId = orderDTO.ShippingAddressId.Value;
            }
            else
            {
                // Nếu ShippingAddressId không được cung cấp hoặc bằng 0, đảm bảo chi tiết NewAddress được cung cấp.
                if (orderDTO.NewAddress == null)
                {
                    // Trả về 400 Bad Request nếu NewAddress bị thiếu.
                    return BadRequest("Chi tiết địa chỉ mới là bắt buộc khi ShippingAddressId không được cung cấp hoặc bằng 0.");
                }
                // Tạo một entity Address mới bằng cách sử dụng chi tiết từ NewAddress DTO.
                var newAddress = new Address
                {
                    City = orderDTO.NewAddress.City,
                    State = orderDTO.NewAddress.State,
                    ZipCode = orderDTO.NewAddress.ZipCode,
                    CustomerId = orderDTO.CustomerId
                };
                // Thêm entity Address mới vào database context.
                _context.Addresses.Add(newAddress);
                // Lưu các thay đổi vào cơ sở dữ liệu để tạo Id cho Address mới.
                await _context.SaveChangesAsync();
                // Gán Id của Address vừa tạo cho shippingAddressId.
                shippingAddressId = newAddress.Id;
            }
            // Tạo một entity Order mới bằng cách sử dụng chi tiết từ OrderDTO và ShippingAddressId đã xác định.
            var order = new Order
            {
                CustomerId = orderDTO.CustomerId,            // Thiết lập CustomerId.
                ShippingAddressId = shippingAddressId,       // Thiết lập ShippingAddressId đã xác định ở trên.
                OrderDate = DateTime.UtcNow,                 // Thiết lập OrderDate là thời gian UTC hiện tại.
                OrderItems = new List<OrderItem>()           // Khởi tạo tập hợp OrderItems.
            };
            decimal orderAmount = 0M;  // Khởi tạo một biến để tích lũy tổng số tiền đơn hàng.
            // Lặp qua mỗi OrderItemDTO trong OrderDTO.
            foreach (var itemDTO in orderDTO.OrderItems)
            {
                // Kiểm tra xem sản phẩm có tồn tại trong cơ sở dữ liệu bằng ProductId không.
                var product = await _context.Products.FindAsync(itemDTO.ProductId);
                if (product == null)
                {
                    // Trả về 400 Bad Request nếu sản phẩm không tồn tại.
                    return BadRequest($"Sản phẩm với ID {itemDTO.ProductId} không tồn tại.");
                }
                // Kiểm tra xem sản phẩm có đủ hàng tồn kho cho số lượng yêu cầu không.
                if (product.Quantity < itemDTO.Quantity)
                {
                    // Trả về 400 Bad Request nếu không đủ hàng tồn kho.
                    return BadRequest($"Không đủ hàng tồn kho cho sản phẩm '{product.Name}'. Số lượng còn lại: {product.Quantity}.");
                }
                // Trừ số lượng đã đặt hàng khỏi số lượng tồn kho của sản phẩm.
                product.Quantity -= itemDTO.Quantity;
                // Tạo một entity OrderItem mới bằng cách sử dụng chi tiết sản phẩm.
                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = itemDTO.Quantity,
                    ProductPrice = product.Price
                };
                // Thêm OrderItem mới vào tập hợp các mặt hàng của Order.
                order.OrderItems.Add(orderItem);
                // Tích lũy tổng số tiền đơn hàng (giá nhân với số lượng).
                orderAmount += product.Price * itemDTO.Quantity;
            }
            // Thiết lập tổng số tiền đơn hàng đã tính toán trên entity Order.
            order.OrderAmount = orderAmount;
            // Thêm entity Order mới vào database context.
            _context.Orders.Add(order);
            // Lưu các thay đổi vào cơ sở dữ liệu để lưu đơn hàng mới và các mặt hàng của nó.
            await _context.SaveChangesAsync();
            // Trả về phản hồi 200 OK với OrderId và một thông báo thành công.
            return Ok(new { OrderId = order.Id, Message = "Tạo đơn hàng thành công." });
        }
    }
}