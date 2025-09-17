using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceDIExample.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceDIExample.Services
{
    public interface IShoppingCartService
    {
        // Retrieves the active cart for a user or creates one if it doesn't exist.
        Cart GetOrCreateCart(int userId);
        // Adds an item to the user's cart.
        void AddItemToCart(int userId, int productId, int quantity = 1);
        // Removes an item from the user's cart.
        void RemoveItemFromCart(int userId, int cartItemId);
        // Updates the quantity of an item in the user's cart.
        void UpdateItemQuantity(int userId, int cartItemId, int newQuantity);
        // Retrieves the user's cart with all items and product details.
        Cart? GetUserCartWithItems(int userId);
        // Clears all items from the user's cart.
        void ClearCart(int userId);
        // Checks out the user's cart, marking it as completed.
        void Checkout(int userId);
    }

    
    // Service có scope (Scoped) để quản lý các thao tác giỏ hàng cho một user cụ thể.
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ILoggerService _logger;

        public ShoppingCartService(ECommerceDbContext dbContext, ILoggerService logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            _logger.Log("ShoppingCartService instance created."); // Log: instance mới được tạo
        }

        // Lấy giỏ hàng đang hoạt động hoặc tạo mới nếu chưa tồn tại
        public Cart GetOrCreateCart(int userId)
        {
            var cart = _dbContext.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == userId && !c.IsCheckedOut);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsCheckedOut = false
                };
                _dbContext.Carts.Add(cart);
                _dbContext.SaveChanges();
                // _logger.Log($"Đã tạo giỏ hàng mới cho User ID: {userId}");
            }
            else
            {
                // _logger.Log($"Lấy giỏ hàng sẵn có (ID: {cart.Id}) của User ID: {userId}");
            }

            return cart;
        }

        // Thêm sản phẩm vào giỏ hàng
        public void AddItemToCart(int userId, int productId, int quantity = 1)
        {
            var cart = GetOrCreateCart(userId);
            var product = _dbContext.Products.Find(productId);

            if (product == null || !product.IsAvailable)
            {
                // _logger.Log($"Cố gắng thêm sản phẩm không khả dụng (ID: {productId})");
                throw new Exception("Product not available.");
            }

            if (product.Stock < quantity)
            {
                // _logger.Log($"Không đủ tồn kho cho sản phẩm (ID: {productId})");
                throw new Exception("Insufficient stock for the product.");
            }

            var existingCartItem = _dbContext.CartItems
                .FirstOrDefault(ci => ci.CartId == cart.Id && ci.ProductId == productId);

            if (existingCartItem == null)
            {
                var newCartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = product.Id,
                    Quantity = quantity,
                    UnitPrice = product.Price,
                    TotalPrice = product.Price * quantity,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _dbContext.CartItems.Add(newCartItem);
                // _logger.Log($"Thêm sản phẩm mới (Product ID: {productId}, Qty: {quantity}) vào giỏ {cart.Id}");
            }
            else
            {
                existingCartItem.Quantity += quantity;
                existingCartItem.TotalPrice = existingCartItem.UnitPrice * existingCartItem.Quantity;
                existingCartItem.UpdatedAt = DateTime.UtcNow;
                _dbContext.CartItems.Update(existingCartItem);
                // _logger.Log($"Cập nhật số lượng sản phẩm (CartItem ID: {existingCartItem.Id})");
            }

            // Giảm tồn kho sản phẩm
            product.Stock -= quantity;
            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();
        }

        // Xóa một sản phẩm khỏi giỏ hàng
        public void RemoveItemFromCart(int userId, int cartItemId)
        {
            var cart = GetOrCreateCart(userId);
            var cartItem = _dbContext.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefault(ci => ci.Id == cartItemId && ci.CartId == cart.Id);

            if (cartItem == null)
            {
                // _logger.Log($"Cố gắng xóa CartItem không tồn tại (ID: {cartItemId})");
                throw new Exception("Cart item not found.");
            }

            // Trả lại tồn kho cho sản phẩm
            if (cartItem.Product != null)
            {
                cartItem.Product.Stock += cartItem.Quantity;
                _dbContext.Products.Update(cartItem.Product);
            }

            _dbContext.CartItems.Remove(cartItem);
            _dbContext.SaveChanges();
            // _logger.Log($"Đã xóa CartItem ID: {cartItemId}");
        }

        // Cập nhật số lượng sản phẩm trong giỏ hàng
        public void UpdateItemQuantity(int userId, int cartItemId, int newQuantity)
        {
            var cart = GetOrCreateCart(userId);
            var cartItem = _dbContext.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefault(ci => ci.Id == cartItemId && ci.CartId == cart.Id);

            if (cartItem == null)
            {
                // _logger.Log($"Cố gắng cập nhật CartItem không tồn tại (ID: {cartItemId})");
                throw new Exception("Cart item not found.");
            }

            if (newQuantity <= 0)
            {
                RemoveItemFromCart(userId, cartItemId);
                return;
            }

            var difference = newQuantity - cartItem.Quantity;

            if (cartItem.Product == null)
            {
                throw new Exception("Associated product not found.");
            }

            if (difference > 0 && cartItem.Product.Stock < difference)
            {
                throw new Exception("Insufficient stock to increase quantity.");
            }

            cartItem.Quantity = newQuantity;
            cartItem.TotalPrice = cartItem.UnitPrice * newQuantity;
            cartItem.UpdatedAt = DateTime.UtcNow;

            // Điều chỉnh tồn kho
            cartItem.Product.Stock -= difference;
            _dbContext.Products.Update(cartItem.Product);
            _dbContext.CartItems.Update(cartItem);
            _dbContext.SaveChanges();
        }

        // Lấy giỏ hàng kèm theo danh sách sản phẩm
        public Cart? GetUserCartWithItems(int userId)
        {
            var cart = _dbContext.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefault(c => c.UserId == userId && !c.IsCheckedOut);

            if (cart != null)
            {
                // _logger.Log($"Đã lấy giỏ hàng ID: {cart.Id}");
            }
            else
            {
                // _logger.Log($"Không tìm thấy giỏ hàng cho User ID: {userId}");
            }

            return cart;
        }

        // Xóa toàn bộ giỏ hàng
        public void ClearCart(int userId)
        {
            var cart = GetOrCreateCart(userId);
            var cartItems = _dbContext.CartItems
                .Where(ci => ci.CartId == cart.Id)
                .Include(ci => ci.Product)
                .ToList();

            foreach (var item in cartItems)
            {
                if (item.Product != null)
                {
                    item.Product.Stock += item.Quantity;
                    _dbContext.Products.Update(item.Product);
                }
                _dbContext.CartItems.Remove(item);
            }

            _dbContext.SaveChanges();
            // _logger.Log($"Đã xóa toàn bộ giỏ hàng {cart.Id}");
        }

        // Thanh toán giỏ hàng (Checkout)
        public void Checkout(int userId)
        {
            var cart = GetOrCreateCart(userId);

            if (cart.CartItems == null || !cart.CartItems.Any())
            {
                throw new Exception("Cart is empty.");
            }

            cart.IsCheckedOut = true;
            cart.UpdatedAt = DateTime.UtcNow;

            _dbContext.Carts.Update(cart);
            _dbContext.SaveChanges();
            // _logger.Log($"Giỏ hàng {cart.Id} đã checkout");
        }
    }
}