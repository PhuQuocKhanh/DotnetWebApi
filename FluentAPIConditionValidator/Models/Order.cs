using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAPIConditionValidator.Models
{
    // Đại diện cho một đơn hàng do khách hàng đặt.
    public class Order
    {
        public int OrderId { get; set; } // Khóa chính
        // Khóa ngoại liên kết đến thực thể Customer
        public int CustomerId { get; set; }
        // Thuộc tính điều hướng cho Customer liên quan
        public Customer Customer { get; set; }
        // Phương thức thanh toán: "CreditCard", "UPI", hoặc "Cash"
        public string PaymentMode { get; set; }
        // Số thẻ tín dụng nếu PaymentMode là CreditCard
        public string? CreditCardNumber { get; set; }
        // ID UPI nếu PaymentMode là UPI
        public string? UPIId { get; set; }
        // Tổng số tiền cho đơn hàng
        [Column(TypeName = "decimal(8,2)")]
        public decimal OrderAmount { get; set; }
        // Tỷ lệ chiết khấu áp dụng cho đơn hàng
        [Column(TypeName = "decimal(8,2)")]
        public decimal Discount { get; set; }
        // Ngày đặt hàng
        public DateTime OrderDate { get; set; }
        // Địa chỉ giao hàng
        public string ShippingAddress { get; set; }
    }
}