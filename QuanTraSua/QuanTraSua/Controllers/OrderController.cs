using Microsoft.AspNetCore.Mvc;
using QuanTraSua.Data;
using QuanTraSua.Models;
using System.Text.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace QuanTraSua.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Trang quản lý đơn hàng
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("AdminAuth") != "true")
                return Redirect("/Admin/Login");

            var orders = _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .ToList();
            return View("~/Areas/Admin/Views/Orders/Index.cshtml", orders);
        }

        // 2. Trang Thanh toán (Khách hàng)
        [HttpGet]
        public IActionResult Create()
        {
            var sessionData = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(sessionData)) return RedirectToAction("Menu", "Home");

            var cart = JsonSerializer.Deserialize<List<CartItem>>(sessionData) ?? new List<CartItem>();
            ViewBag.Cart = cart;
            ViewBag.Total = cart.Sum(x => x.Price * x.Quantity);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Order order)
        {
            var sessionData = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(sessionData)) return RedirectToAction("Menu", "Home");

            var cart = JsonSerializer.Deserialize<List<CartItem>>(sessionData) ?? new List<CartItem>();

            // --- BƯỚC 1: KIỂM TRA TỒN KHO TRƯỚC ---
            foreach (var item in cart)
            {
                var product = _context.Products.Find(item.ProductId);
                if (product != null)
                {
                    if (item.Quantity > product.Stock)
                    {
                        // Thêm lỗi vào ModelState để hiển thị ngoài giao diện
                        ModelState.AddModelError("", $"Món '{item.ProductName}' hiện chỉ còn {product.Stock} ly. Vui lòng giảm số lượng.");
                        
                        ViewBag.Cart = cart;
                        ViewBag.Total = cart.Sum(x => x.Price * x.Quantity);
                        return View(order); // Trả về trang thanh toán kèm thông báo lỗi
                    }
                }
            }

            if (ModelState.IsValid)
            {
                // --- BƯỚC 2: NẾU ĐỦ HÀNG THÌ MỚI TRỪ KHO VÀ LƯU ĐƠN ---
                foreach (var item in cart)
                {
                    var product = _context.Products.Find(item.ProductId);
                    if (product != null)
                    {
                        product.Stock -= item.Quantity;
                        _context.Update(product);
                    }
                }

                order.TotalPrice = cart.Sum(x => x.Price * x.Quantity) + 20000;
                order.OrderDate = DateTime.Now;
                order.OrderDetails = string.Join(", ", cart.Select(c => $"{c.ProductName} (x{c.Quantity})"));
                order.Status = "Chờ xác nhận"; 

                _context.Orders.Add(order);
                _context.SaveChanges();

                HttpContext.Session.Remove("Cart");
                return RedirectToAction("Success");
            }

            ViewBag.Cart = cart;
            ViewBag.Total = cart.Sum(x => x.Price * x.Quantity);
            return View(order);
        }

        public IActionResult Success() => View();

        // --- XÁC NHẬN ĐƠN HÀNG ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Order/Confirm/{id}")]
        public IActionResult Confirm(int id)
        {
            if (HttpContext.Session.GetString("AdminAuth") != "true")
                return Redirect("/Admin/Login");

            var order = _context.Orders.Find(id);
            if (order != null)
            {
                order.Status = "Đã xác nhận";
                _context.SaveChanges();
                TempData["Success"] = "Đã xác nhận đơn hàng #" + id;
            }
            else
            {
                TempData["Error"] = "Không tìm thấy đơn hàng để xác nhận.";
            }
            return RedirectToAction(nameof(Index));
        }

        // 3. XỬ LÝ XÓA ĐƠN HÀNG
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Order/DeleteConfirm/{id}")]
        public IActionResult DeleteConfirm(int id)
        {
            if (HttpContext.Session.GetString("AdminAuth") != "true")
            {
                TempData["Error"] = "Bạn không có quyền thực hiện thao tác này.";
                return Redirect("/Admin/Login");
            }

            try
            {
                var order = _context.Orders.Find(id);
                if (order == null)
                {
                    TempData["Error"] = "Không tìm thấy đơn hàng #" + id;
                }
                else
                {
                    _context.Orders.Remove(order);
                    _context.SaveChanges();
                    TempData["Success"] = "Đã xóa thành công đơn hàng #" + id;
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi hệ thống: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}