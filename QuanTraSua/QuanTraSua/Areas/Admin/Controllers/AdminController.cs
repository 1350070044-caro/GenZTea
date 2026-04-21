using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanTraSua.Data;
using System.Linq;
using System.Text.Json;

namespace QuanTraSua.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("AdminAuth") == "true")
                return RedirectToAction("Dashboard");
            return View();
        }

[HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == "admin" && password == "admin123")
            {
                HttpContext.Session.SetString("AdminAuth", "true");
                return RedirectToAction("Dashboard");
            }
            ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu!";
            return View();
        }


public async Task<IActionResult> Dashboard()
        {
            if (!IsAdminAuthenticated()) return RedirectToAction("Login");

var orders = await _context.Orders.ToListAsync();

            // 1. Thống kê tổng quan cho các thẻ trên cùng
            ViewBag.TotalOrders = orders.Count;
            // Chỉ tính doanh thu từ các đơn không phải "Đã hủy" (nếu bạn có trường Status)
            ViewBag.TotalRevenue = orders.Sum(o => o.TotalPrice);
            // Đếm khách hàng dựa trên số điện thoại duy nhất
            ViewBag.TotalCustomers = orders.Select(o => o.Phone).Distinct().Count();

            // 2. Chuẩn bị dữ liệu cho biểu đồ Doanh thu (6 tháng gần nhất)
            var chartLabels = new List<string>();
            var chartData = new List<decimal>();

            for (int i = 5; i >= 0; i--)
            {
                var date = DateTime.Now.AddMonths(-i);
                chartLabels.Add($"Tháng {date.Month}");
                
                var monthlyRevenue = orders
                    .Where(o => o.OrderDate.Month == date.Month && o.OrderDate.Year == date.Year)
                    .Sum(o => o.TotalPrice);
                chartData.Add(monthlyRevenue);
            }

            ViewBag.ChartLabels = JsonSerializer.Serialize(chartLabels);
            ViewBag.ChartData = JsonSerializer.Serialize(chartData);

            // 3. Thống kê trạng thái đơn hàng (Ví dụ: Chờ, Đã xác nhận)
            // Lưu ý: Nếu Model Order của bạn chưa có trường Status, hãy mặc định chia theo logic riêng hoặc thêm trường Status vào DB
            ViewBag.Pending = orders.Count(o => o.TotalPrice > 0); // Thay bằng logic Status nếu có
            ViewBag.Confirmed = orders.Count(o => o.TotalPrice > 100000); // Ví dụ logic phân loại

            // 4. Top 5 sản phẩm random cho chart
            var allProducts = await _context.Products.ToListAsync();
            var random = new Random();
            var topProducts = allProducts.OrderBy(x => random.Next()).Take(5).ToList();
            var topLabels = topProducts.Select(p => p.Name).ToList();
            var topData = topProducts.Select(p => random.Next(10, 51)).ToList();
            ViewBag.TopProductLabels = JsonSerializer.Serialize(topLabels);
            ViewBag.TopProductData = JsonSerializer.Serialize(topData);

var recentOrders = await _context.Orders.OrderByDescending(o => o.OrderDate).Take(10).ToListAsync();
            return View(recentOrders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrder(QuanTraSua.Models.Order order)
        {
            if (!IsAdminAuthenticated()) return Unauthorized();

            if (ModelState.IsValid)
            {
                order.OrderDate = DateTime.Now;
                _context.Orders.Add(order);
                _context.SaveChanges();
                TempData["Success"] = "Đã thêm đơn hàng thành công!";
            }
            else
            {
                TempData["Error"] = "Dữ liệu không hợp lệ.";
            }
            return RedirectToAction("Dashboard");
        }

        public IActionResult Products()
        {
            if (!IsAdminAuthenticated()) return RedirectToAction("Login");
            return Redirect("/Product");
        }

        public IActionResult Orders()
        {
            if (!IsAdminAuthenticated()) return RedirectToAction("Login");
            return Redirect("/Order");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminAuth");
            return RedirectToAction("Menu", "Home");
        }


        private bool IsAdminAuthenticated()
        {
            return HttpContext.Session.GetString("AdminAuth") == "true";
        }
    }
}