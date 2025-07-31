using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapperIgnoreProperty.DTOs;
using AutoMapperIgnoreProperty.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoMapperIgnoreProperty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductIgnoreAllValueNullController : ControllerBase
    {
        private readonly IMapper _mapper;

        public ProductIgnoreAllValueNullController(IMapper mapper)
        {
            _mapper = mapper;
        }

        private List<ProductIgnoreAllValueNull> listProducts = new List<ProductIgnoreAllValueNull>()
        {
            new () { Id = 1001, Name = "Laptop", Price = 1000 },
            new () { Id = 1002, Name = "Desktop", Price = 2000 }
        };

        [HttpPut("{Id}")]
        public ActionResult<ProductIgnoreAllValueNull> UpdateProduct(int Id, ProductDTOIgnoreAllValueNull productDTO)
        {
            var product = listProducts.FirstOrDefault(p => p.Id == Id);
            if (product == null)
                return NotFound("Product Not Found");

            // AutoMapper sẽ bỏ qua các giá trị null từ DTO
            _mapper.Map(productDTO, product);

            return Ok(product);
        }
    }
}