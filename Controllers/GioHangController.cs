using DA1CNTT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DA1CNTT.Controllers
{
    public class GioHangController : Controller
    {
        private readonly ShopquanaoContext _context;
        public GioHangController(ShopquanaoContext context)
        {
            _context = context;
        }

        private User GetCurrentUser()
        {
            var email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email))
            return null;

            return _context.Users.FirstOrDefault(nd => nd.Email == email);
        }
        public IActionResult Index()
        {
            var nguoiDung = GetCurrentUser();
            if (nguoiDung == null)
            {
                TempData["Loi"] = "Bạn cần đăng nhập để xem giỏ hàng.";
                return RedirectToAction("Login", "Customer");
            }

            if (_context.Chitietgiohangs == null)
            {
                TempData["Loi"] = "Lỗi hệ thống: không tìm thấy dữ liệu giỏ hàng.";
                return View(new List<Chitietgiohang>());
            }

            // Kiểm tra _context.GioHangChiTiets có null không
            if (_context.Chitietgiohangs == null)
            {
                TempData["Loi"] = "Lỗi hệ thống: không tìm thấy dữ liệu giỏ hàng.";
                return View(new List<Chitietgiohang>()); // Trả về giỏ hàng rỗng
            }

            var gioHang = _context.Chitietgiohangs
                .Include(g => g.IdSanphamNavigation)
                .Where(g => g.IdUsers == nguoiDung.IdUsers && g.IdSanphamNavigation != null) // 🔥 lọc item rác
                .ToList();

            return View(gioHang);
        }

        public IActionResult GioHangChiTiet()
        {
            var nguoiDung = GetCurrentUser();
            if (nguoiDung == null)
            {
                return RedirectToAction("Login", "Customer");
            }
            var gioHang = _context.Chitietgiohangs
                .Include(g => g.IdSanphamNavigation)
                .Where(g => g.IdUsers == nguoiDung.IdUsers && g.IdSanphamNavigation != null) // 🔥 lọc item rác
                .ToList();

            return View(gioHang);
        }

        [HttpPost]
        public IActionResult ThemVaoGio(string MaSanPham, string Size, int SoLuong)
        {
            var nguoiDung = GetCurrentUser();
            if (nguoiDung == null)
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng.";
                return RedirectToAction("Login", "Customer");
            }

            var sanPham = _context.Sanphams.FirstOrDefault(sp => sp.IdSanpham == MaSanPham);
            if (sanPham == null)
            {
                TempData["ErrorMessage"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Index", "SanPham");
            }


            if (sanPham.SoLuongTonKho < SoLuong || sanPham.SoLuongTonKho == 0)
            {
                TempData["ErrorMessage"] = "Sản phẩm đã hết hàng.";
                return RedirectToAction("Index", "SanPham");
            }


            var existingItem = _context.Chitietgiohangs.FirstOrDefault(
               x => x.IdUsers == nguoiDung.IdUsers &&
                    x.IdSanpham == MaSanPham
           );

            if (existingItem != null)
            {
                existingItem.Soluong += SoLuong;
            }
            else
            {
                var gioHangChiTiet = new Chitietgiohang
                {
                    IdChitietGiohang = Guid.NewGuid().ToString("N").Substring(0, 20),
                    IdUsers = nguoiDung.IdUsers,
                    IdSanpham = MaSanPham,
                    Soluong = SoLuong,
                    NgayTao = DateTime.Now,
                };
                _context.Chitietgiohangs.Add(gioHangChiTiet);
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Đã thêm sản phẩm vào giỏ hàng.";
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult CapNhatSoLuong(string maChiTietGH, int soLuong)
        {
            var chiTiet = _context.Chitietgiohangs.FirstOrDefault(x => x.IdChitietGiohang == maChiTietGH);
            if (chiTiet != null && soLuong > 0)
            {
                chiTiet.Soluong = soLuong;
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "GioHang");
        }


        [HttpPost]
        public IActionResult Xoa(string maChiTietGH)
        {
            var item = _context.Chitietgiohangs.Find(maChiTietGH);
            if (item != null)
            {
                _context.Chitietgiohangs.Remove(item);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ThanhToan(string HoTen, string DiaChi, string SoDienThoai, string GhiChu, string PhuongThuc)
        {


            var nguoiDung = GetCurrentUser();
            if (nguoiDung == null)
            {
                TempData["Loi"] = "Bạn cần đăng nhập để thanh toán.";
                return RedirectToAction("Login", "Customer");
            }

            var gioHang = _context.Chitietgiohangs
                .Include(g => g.IdSanphamNavigation)
                .Where(g => g.IdUsers == nguoiDung.IdUsers && g.IdSanphamNavigation != null) // 🔥 lọc item rác
                .ToList();


            if (gioHang == null || !gioHang.Any())
            {
                TempData["ThongBao"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index");
            }

            foreach (var item in gioHang)
            {
                var sanPham = await _context.Sanphams.FindAsync(item.IdSanpham);
                if (sanPham == null || sanPham.SoLuongTonKho < item.Soluong)
                {
                    TempData["Loi"] = $"Sản phẩm {item.IdSanphamNavigation?.IdSanpham} không đủ số lượng trong kho.";
                    return RedirectToAction("Index");
                }
            }

            decimal tongTien = 0;
            foreach (var item in gioHang)
            {
                tongTien += (decimal)(item.IdSanphamNavigation.Gia * item.Soluong);
            }



            try
            {
                var donHang = new Donhang
                {
                    IdDonhang = Guid.NewGuid().ToString("N").Substring(0, 10),
                    IdUsers = nguoiDung.IdUsers,
                    Ngaydathang = DateTime.Now,
                    Trangthai = "DANGXULY"
                };

                _context.Donhangs.Add(donHang);

                foreach (var item in gioHang)
                {
                    var gia = item.IdSanphamNavigation?.Gia ?? 0;
                    var chiTiet = new Chitietdonhang
                    {
                        IdOrder = Guid.NewGuid().ToString("N").Substring(0, 10),
                        IdDonhang = donHang.IdDonhang,
                        IdSanpham = item.IdSanpham,
                        Soluong = item.Soluong,
                        Giadat = gia
                    };

                    _context.Chitietdonhangs.Add(chiTiet);

                    var sanPham = await _context.Sanphams.FindAsync(item.IdSanpham);
                    if (sanPham != null)
                    {
                        sanPham.SoLuongTonKho -= item.Soluong;
                    }
                }

                _context.Chitietgiohangs.RemoveRange(gioHang);
                await _context.SaveChangesAsync();
                TempData["ThongBao"] = "Đặt hàng thành công!";

                return RedirectToAction("OrderSuccessful", "Donhangs", new { id = donHang.IdDonhang });
            }
            catch (Exception ex)
            {
                TempData["Loi"] = "Có lỗi xảy ra khi đặt hàng: " + ex.Message;
                return RedirectToAction("ThongTinDonHang");
            }
        }


        public IActionResult ThongTinDonHang()
        {
            var nguoiDung = GetCurrentUser();
            if (nguoiDung == null)
            {
                TempData["Loi"] = "Bạn cần đăng nhập để xem thông tin đơn hàng.";
                return RedirectToAction("Login", "Customer");
            }
            var gioHang = _context.Chitietgiohangs
                .Include(g => g.IdSanphamNavigation)
                .Where(g => g.IdUsers == nguoiDung.IdUsers && g.IdSanphamNavigation != null) // 🔥 lọc item rác
                .ToList();

            if (!gioHang.Any())
            {
                TempData["Loi"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index");
            }

            decimal tongTien = 0;
            foreach (var item in gioHang)
            {
                tongTien += (decimal)(item.IdSanphamNavigation.Gia * item.Soluong);
            }

            string maDonHang = "DH" + DateTime.Now.Ticks.ToString();

            ViewBag.NguoiDung = nguoiDung;
            ViewBag.TongTien = tongTien;
            ViewBag.MaDonHang = maDonHang;

            return View();
        }



    }
}