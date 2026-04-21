using Microsoft.AspNetCore.Mvc;
using QuanTraSua.Data;
using QuanTraSua.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace QuanTraSua.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CartController(ApplicationDbContext context) { _context = context; }

        private List<CartItem> GetCartItems() {
            var sessionData = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(sessionData)) return new List<CartItem>();
            return JsonSerializer.Deserialize<List<CartItem>>(sessionData) ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart) {
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
        }

        public IActionResult Index() {
            var cart = GetCartItems();
            ViewBag.Total = cart.Sum(s => s.Price * s.Quantity);
            return View(cart);
        }

        public IActionResult AddToCart(int id, int quantity = 1) {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            var cart = GetCartItems();
            var item = cart.FirstOrDefault(c => c.ProductId == id);

            if (item == null) {
                // Giới hạn số lượng mua không vượt quá tồn kho ngay khi thêm mới
                int finalQuantity = quantity > product.Stock ? product.Stock : quantity;
                if (finalQuantity < 1 && product.Stock > 0) finalQuantity = 1;

                cart.Add(new CartItem {
                    ProductId = id, 
                    ProductName = product.Name ?? "Sản phẩm", 
                    Price = product.Price, 
                    Quantity = finalQuantity, 
                    ImageUrl = product.ImageUrl ?? "",
                    Stock = product.Stock // LẤY DỮ LIỆU KHO TỪ DATABASE
                });
            } else {
                // Kiểm tra cộng dồn không được vượt quá kho
                if (item.Quantity + quantity > product.Stock) {
                    item.Quantity = product.Stock;
                } else {
                    item.Quantity += quantity;
                }
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        // --- HÀM MỚI: XỬ LÝ TĂNG GIẢM SỐ LƯỢNG TỪ NÚT + / - ---
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var cart = GetCartItems();
            var item = cart.FirstOrDefault(c => c.ProductId == id);

            if (item != null)
            {
                // Giới hạn từ 1 đến 99
                if (quantity < 1) quantity = 1;
                if (quantity > 99) quantity = 99;

                // Kiểm tra với số lượng tồn kho thực tế
                if (quantity > item.Stock)
                {
                    quantity = item.Stock;
                }

                item.Quantity = quantity;
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id) {
            var cart = GetCartItems();
            cart.RemoveAll(c => c.ProductId == id);
            SaveCart(cart);
            return RedirectToAction("Index");
        }
    }

    public class CartItem {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int Stock { get; set; } // Kho đã được thêm vào đây
    }
}