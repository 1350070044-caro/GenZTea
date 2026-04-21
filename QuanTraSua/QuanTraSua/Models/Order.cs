using System;
using System.ComponentModel.DataAnnotations;

namespace QuanTraSua.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string Phone { get; set; } = string.Empty;

        public string? Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        public string Address { get; set; } = string.Empty;

        public string? Note { get; set; }

        public string PaymentMethod { get; set; } = "COD";

        public decimal TotalPrice { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        // Lưu danh sách món dưới dạng text: "Trà sữa (x2), Hồng trà (x1)"
        public string OrderDetails { get; set; } = string.Empty;
        // Mở file Models/Order.cs và thêm dòng này vào
        public string Status { get; set; } = "Chờ xác nhận";
    }
}