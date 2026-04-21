using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanTraSua.Data;
using QuanTraSua.Models;
using System.Diagnostics;
using System.Linq; // QUAN TRỌNG: Để sử dụng .Sum(), .Count()

namespace QuanTraSua.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Doanh Thu
        public IActionResult Index()
        {
            var topProducts = _context.Products.Take(8).ToList();
            return View(topProducts);
        }



        public IActionResult Menu()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}