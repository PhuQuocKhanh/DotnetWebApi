using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAPIConditionValidator.DTOs
{
    // DTO để tạo một đơn hàng mới. Đóng gói dữ liệu đơn hàng từ client.
    public class OrderDTO
    {
        public int CustomerId { get; set; }
        // PaymentMode phải là "CreditCard", "UPI", hoặc "Cash"
        public string PaymentMode { get; set; }
        // Bắt buộc nếu PaymentMode là "CreditCard"
        public string? CreditCardNumber { get; set; }
        // Bắt buộc nếu PaymentMode là "UPI"
        public string? UPIId { get; set; }
        // Tổng số tiền đơn hàng; phải lớn hơn không
        public decimal OrderAmount { get; set; }
        // Tỷ lệ chiết khấu; được xác thực có điều kiện
        public decimal Discount { get; set; }
        // Địa chỉ giao hàng là bắt buộc để xử lý đơn hàng
        public string ShippingAddress { get; set; }
    }
}