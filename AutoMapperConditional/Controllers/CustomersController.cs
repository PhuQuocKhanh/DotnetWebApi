using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapperConditional.Data;
using AutoMapperConditional.DTOs;
using AutoMapperConditional.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoMapperConditional.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ECommerceDbContext _context;
        private readonly IMapper _mapper;

        public CustomersController(ECommerceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/customers
        [HttpGet("GetCustomers")]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomers()
        {
            var customers = await _context.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(i => i.Product)
                .ToListAsync();

            var customerDtos = _mapper.Map<IEnumerable<CustomerDTO>>(customers);
            return Ok(customerDtos);
        }

        // GET: api/customers/1
        [HttpGet("GetCustomerById/{id}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomerById(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null) return NotFound();

            var customerDto = _mapper.Map<CustomerDTO>(customer);
            return Ok(customerDto);
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult<CustomerDTO>> CreateCustomer(CustomerDTO customerDto)
        {
            var customer = new Customer
            {
                Name = customerDto.Name ?? "New Customer",
                IsActive = true // mặc định là đang hoạt động
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var createdDto = _mapper.Map<CustomerDTO>(customer);
            return Ok(createdDto);
        }
    }
}