using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapperComplexMappingDemo.Data;
using AutoMapperComplexMappingDemo.DTOs;
using AutoMapperComplexMappingDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoMapperComplexMappingDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
         private readonly ECommerceDBContext _context;
        private readonly IMapper _mapper;

        public OrderController(ECommerceDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // POST: api/order
        [HttpPost]
        public async Task<ActionResult<OrderDTO>> CreateOrder([FromBody] OrderCreateDTO orderCreateDTO)
        {
            if (orderCreateDTO == null)
                return BadRequest("Order data is null.");

            try
            {
                // Kiểm tra xem Customer có tồn tại không
                var customerExists = await _context.Customers.AnyAsync(c => c.Id == orderCreateDTO.CustomerId);
                if (!customerExists)
                    return NotFound($"Customer with ID {orderCreateDTO.CustomerId} not found.");

                // Lấy danh sách Product từ danh sách ID
                var productIds = orderCreateDTO.Items.Select(i => i.ProductId).ToList();
                var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();

                if (products.Count != productIds.Count)
                    return BadRequest("One or more products in the order are invalid.");

                // Ánh xạ DTO sang Entity
                var order = _mapper.Map<Order>(orderCreateDTO);

                // Tính tổng tiền đơn hàng
                decimal totalAmount = 0;
                foreach (var item in order.OrderItems)
                {
                    var product = products.First(p => p.Id == item.ProductId);
                    totalAmount += product.Price * item.Quantity;
                }
                order.Amount = totalAmount;

                // Lưu vào DB
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Lấy lại dữ liệu sau khi lưu (bao gồm liên kết với Customer, Address, Product,...)
                var createdOrder = await _context.Orders
                    .Include(o => o.Customer)
                    .ThenInclude(c => c.Address)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(o => o.Id == order.Id);

                if (createdOrder == null)
                    return StatusCode(500, "An error occurred while creating the order.");

                // Trả về DTO
                var orderDTO = _mapper.Map<OrderDTO>(createdOrder);
                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, orderDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
            }
        }

        // GET: api/order/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrderById(int id)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.Customer)
                    .ThenInclude(c => c.Address)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                    return NotFound($"Order with ID {id} not found.");

                var orderDTO = _mapper.Map<OrderDTO>(order);
                return Ok(orderDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching the order: {ex.Message}");
            }
        }

        // GET: api/order/customer/{customerId}
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrdersByCustomerId(int customerId)
        {
            try
            {
                var orders = await _context.Orders
                    .Where(o => o.CustomerId == customerId)
                    .Include(o => o.Customer)
                    .ThenInclude(c => c.Address)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ToListAsync();

                if (orders == null || orders.Count == 0)
                    return NotFound($"No orders found for customer with ID {customerId}.");

                var ordersDTO = _mapper.Map<IEnumerable<OrderDTO>>(orders);
                return Ok(ordersDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching orders for customer ID {customerId}: {ex.Message}");
            }
        }
    }
}