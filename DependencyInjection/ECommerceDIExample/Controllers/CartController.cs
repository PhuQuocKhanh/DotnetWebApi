using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceDIExample.DTOs;
using ECommerceDIExample.Models;
using ECommerceDIExample.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceDIExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IGlobalDiscountService _globalDiscountService;
        private readonly IOrderIdGenerator _orderIdGenerator;
        private readonly ECommerceDbContext _context;

        public CartController(
            IShoppingCartService shoppingCartService,
            IGlobalDiscountService globalDiscountService,
            IOrderIdGenerator orderIdGenerator,
            ECommerceDbContext context)
        {
            _shoppingCartService = shoppingCartService;
            _globalDiscountService = globalDiscountService;
            _orderIdGenerator = orderIdGenerator;
            _context = context;
        }

        // Thêm sản phẩm vào giỏ hàng
        [HttpPost("AddItem")]
        public IActionResult AddItem([FromBody] AddItemRequestDTO request)
        {
            try
            {
                _shoppingCartService.AddItemToCart(request.UserId, request.ProductId, request.Quantity);
                return Ok("Item added to cart.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Xóa sản phẩm khỏi giỏ hàng
        [HttpDelete("RemoveItem")]
        public IActionResult RemoveItem(RemoveItemRequestDTO requestDTO)
        {
            try
            {
                _shoppingCartService.RemoveItemFromCart(requestDTO.UserId, requestDTO.CartItemId);
                return Ok("Item removed from cart.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Cập nhật số lượng sản phẩm trong giỏ
        [HttpPut("UpdateItemQuantity")]
        public IActionResult UpdateItemQuantity([FromBody] UpdateQuantityRequestDTO request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _shoppingCartService.UpdateItemQuantity(request.UserId, request.CartItemId, request.NewQuantity);
                return Ok("Cart item quantity updated.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Lấy giỏ hàng của user
        [HttpGet("{userId}")]
        public IActionResult GetUserCart(int userId)
        {
            var cart = _shoppingCartService.GetUserCartWithItems(userId);
            if (cart == null)
            {
                return NotFound("Cart not found for this user.");
            }

            // Lấy loại user từ DB
            var userType = _context.Users.Find(userId)?.UserType;
            // Áp dụng giảm giá toàn cục
            var discountRate = _globalDiscountService.GetDiscountRate(userType);

            // Map sang DTO
            var cartDto = new CartResponseDTO
            {
                CartId = cart.Id,
                UserId = cart.UserId,
                IsCheckedOut = cart.IsCheckedOut,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                Items = cart.CartItems?.Select(ci => new CartItemResponseDTO
                {
                    CartItemId = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.Name,
                    Description = ci.Product?.Description,
                    UnitPrice = ci.UnitPrice,
                    TotalPrice = ci.TotalPrice,
                    DiscountedPrice = (ci.UnitPrice * ci.Quantity) * (1 - discountRate),
                    Quantity = ci.Quantity
                }),
                TotalAmount = cart.CartItems?.Sum(ci => ci.TotalPrice) ?? 0m,
                DiscountRate = discountRate,
                DiscountedTotal = (cart.CartItems?.Sum(ci => ci.TotalPrice) ?? 0m) * (1 - discountRate)
            };
            return Ok(cartDto);
        }

        // Xóa toàn bộ giỏ hàng
        [HttpPost("ClearCart/{userId}")]
        public IActionResult ClearCart(int userId)
        {
            try
            {
                _shoppingCartService.ClearCart(userId);
                return Ok("Cart cleared successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Thanh toán giỏ hàng
        [HttpPost("checkout/{userId}")]
        public IActionResult Checkout(int userId)
        {
            try
            {
                _shoppingCartService.Checkout(userId);
                var orderId = _orderIdGenerator.GenerateOrderId();
                return Ok(new { Message = "Checkout successful.", OrderId = orderId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}