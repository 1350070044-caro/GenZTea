using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanTraSua.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace QuanTraSua.Controllers
{
    [Route("Chart")]
    public class ChartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("MonthlyRevenue")]
        public IActionResult MonthlyRevenue()
        {
            // 1. Lấy năm hiện tại
            int currentYear = DateTime.Now.Year;

            // 2. Lấy dữ liệu từ Database (Chỉ lọc theo năm để tối ưu)
            var ordersInYear = _context.Orders
                .Where(o => o.OrderDate.Year == currentYear)
                .Select(o => new { o.OrderDate.Month, o.TotalPrice })
                .ToList(); // Đưa về bộ nhớ để tránh lỗi dịch LINQ to SQL

            // 3. Nhóm dữ liệu và tính tổng doanh thu theo tháng
            var monthlyData = ordersInYear
                .GroupBy(o => o.Month)
                .Select(g => new {
                    Month = g.Key,
                    Revenue = g.Sum(o => (double)o.TotalPrice)
                })
                .OrderBy(x => x.Month)
                .ToList();

            // 4. Chuẩn bị mảng 12 tháng để biểu đồ luôn đầy đủ (kể cả tháng chưa có doanh thu)
            var labels = new string[12];
            var data = new double[12];

            for (int i = 1; i <= 12; i++)
            {
                labels[i - 1] = $"Tháng {i:00}"; // Định dạng: Tháng 01, Tháng 02...
                
                // Tìm doanh thu của tháng i trong dữ liệu đã nhóm
                var monthRecord = monthlyData.FirstOrDefault(x => x.Month == i);
                data[i - 1] = monthRecord != null ? monthRecord.Revenue : 0;
            }

            // 5. Trả về JSON cho biểu đồ
            return Json(new { labels, data });
        }
    }
}