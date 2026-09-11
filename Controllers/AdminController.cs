using DA1CNTT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DA1CNTT.Controllers
{
    public class AdminController : Controller
    {
        private readonly ShopquanaoContext _context;
        public AdminController(ShopquanaoContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {


            var today = DateTime.Today;

            // Doanh thu hôm nay
            var revenueToday = _context.Chitietdonhangs
               .Where(ct => ct.IdDonhangNavigation.Ngaydathang >= today
                         && ct.IdDonhangNavigation.Ngaydathang < today.AddDays(1)
                         && ct.IdDonhangNavigation.Trangthai == "Hoàn thành")
               .Sum(ct => (decimal?)(ct.Soluong * ct.Giadat)) ?? 0;
            // Tổng đơn hàng
            var totalOrders = _context.Donhangs.Count();

            // Tổng người dùng
            var totalUsers = _context.Users.Count();

            // Sản phẩm bán chạy (top 5)
            var topProducts = _context.Chitietdonhangs
                .GroupBy(od => od.IdSanpham)
                .Select(g => new {
                    ProductId = g.Key,
                    Quantity = g.Sum(x => x.Soluong)
                })
                .OrderByDescending(g => g.Quantity)
                .Take(5)
                .ToList();

            int currentYear = DateTime.Now.Year;

            var revenueByMonth = _context.Donhangs
                .Where(o => o.Trangthai == "Hoàn thành"
                         && o.Ngaydathang.HasValue
                         && o.Ngaydathang.Value.Year == currentYear)
                .Select(o => new
                {
                    Month = o.Ngaydathang.Value.Month,
                    Revenue = _context.Chitietdonhangs
                                .Where(ct => ct.IdDonhang == o.IdDonhang)
                                .Sum(ct => ct.Soluong * ct.Giadat)
                })
                .GroupBy(x => x.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Revenue = g.Sum(x => x.Revenue)
                })
                .OrderBy(x => x.Month)
                .ToList();


            ViewBag.RevenueToday = revenueToday;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TopProducts = topProducts;
            ViewBag.RevenueByMonth = revenueByMonth;

            return View();
        }
    }
}
