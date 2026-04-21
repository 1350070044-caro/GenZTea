namespace QuanTraSua.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        
        // CÁC CỘT MỚI ĐƯỢC THÊM VÀO
        public string? ImageUrl { get; set; }   // Link ảnh sản phẩm
        public string? Description { get; set; } // Mô tả mùi vị
        public int Stock { get; set; } = 0;   // Quản Lý Tồn Kho
    }
}