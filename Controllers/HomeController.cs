using System.Diagnostics;
using DA1CNTT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace CNTTDA1.Controllers
{
    public class HomeController : Controller
    {
        ShopquanaoContext db = new ShopquanaoContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int? page)
        {
            
            int pagesize = 4;
            int pagenumber = page == null || page < 0 ? 1 : page.Value;

            var listSPMoi = db.Sanphams.AsNoTracking().Where(x => x.CreateAt >= DateTime.Now.AddDays(-200)).OrderByDescending(x => Guid.NewGuid());
            var listSPNam = db.Sanphams.AsNoTracking().Where(x => x.Sex == "NAM").OrderByDescending(x => Guid.NewGuid());
            var listSPNU = db.Sanphams.AsNoTracking().Where(x => x.Sex == "NU").OrderByDescending(x => Guid.NewGuid());

            PagedList<Sanpham> lst1 = new PagedList<Sanpham> (listSPMoi,pagenumber, pagesize);
            PagedList<Sanpham> lst2 = new PagedList<Sanpham>(listSPNam, pagenumber, pagesize);
            PagedList<Sanpham> lst3 = new PagedList<Sanpham>(listSPNU, pagenumber, pagesize);

            ViewBag.SanPhamMoi = lst1;
            ViewBag.SanPhamChoNam = lst2;
            ViewBag.SanPhamChoNu = lst3;
            return View();
        }



        public IActionResult ChinhSach()
        {
            return View();
        }

        public IActionResult HuongDan()
        {
            return View();
        }

        public IActionResult PhuongThucThanhToan()
        {
            return View();
        }

        public IActionResult ChinhSachVanChuyen()
        {
            return View();
        }

        public IActionResult Thongtinlienhe()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
