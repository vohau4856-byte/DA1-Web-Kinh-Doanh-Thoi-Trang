using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DA1CNTT.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.VisualStudio.TextTemplating;
using Microsoft.CodeAnalysis.Scripting;

namespace DA1CNTT.Controllers
{
    public class CustomerController : Controller
    {

        private readonly ShopquanaoContext _context;

        ShopquanaoContext db = new ShopquanaoContext();
        public CustomerController(ShopquanaoContext context)
        {
            _context = context;
        }


        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string Username, string PasswordUsers)
        {
            try
            {
                // Kiểm tra input
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(PasswordUsers))
                {
                    ViewBag.Error = "Vui lòng nhập đầy đủ thông tin!";
                    return View();
                }

                // Kiểm tra database connection
                var totalUsers = await _context.Users.CountAsync();
                if (totalUsers == 0)
                {
                    ViewBag.Error = "Database không có user nào!";
                    return View();
                }

                // Tìm user đơn giản - không case sensitive
                var user = await _context.Users
                    .Where(u => u.Username.ToLower() == Username.ToLower() || u.Email.ToLower() == Username.ToLower())
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng!";
                    return View();
                }

                // Kiểm tra password
                if (user.PasswordUsers != PasswordUsers)
                {
                    ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng!";
                    return View();
                }

                // Login thành công
                HttpContext.Session.SetString("TenDangNhap", user.Username ?? "");
                HttpContext.Session.SetString("Email", user.Email ?? "");
                HttpContext.Session.SetString("VaiTro", user.Roles ?? "");
                HttpContext.Session.SetString("MaNguoiDung", user.IdUsers);


                string displayName = !string.IsNullOrEmpty(user.Tennguoidung) ? user.Tennguoidung : user.Username;
                HttpContext.Session.SetString("DisplayName", displayName ?? "");

                if (user.Roles == "ADMIN")
                {
                    return RedirectToAction("Index", "Admin");
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Lỗi: {ex.Message}";
                return View();
            }
        }

        [HttpGet]

        public IActionResult Logout()
        {
            // Xóa tất cả session
            HttpContext.Session.Clear();

            // Redirect về trang chủ
            return RedirectToAction("Index", "Home");
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password)
        {


            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin!";
                return View();
            }

            // Kiểm tra độ dài password
            if (password.Length < 6)
            {
                ViewBag.Error = "Mật khẩu phải có ít nhất 6 ký tự!";
                return View();
            }


            // Kiểm tra username đã tồn tại chưa
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username || u.Email == email);

            if (existingUser != null)
            {
                ViewBag.Error = "Tên đăng nhập hoặc email đã tồn tại!";
                return View();
            }
            else
            {
                var newUser = new User
                {
                    IdUsers = Guid.NewGuid().ToString("N")[..8],
                    Tennguoidung = username,
                    Username = username,
                    Email = email,
                    ImgUrl = "user.png",
                    PasswordUsers = password,
                    CreateAt = DateTime.Now
                };

                // Thêm vào database
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                ViewBag.Success = "Đăng ký thành công!";
                return RedirectToAction("Login");
            }

            return this.Register();
        }

        // Thêm vào CustomerController

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            try
            {
                // Lấy username từ session
                var username = HttpContext.Session.GetString("TenDangNhap");

                if (string.IsNullOrEmpty(username))
                {
                    return RedirectToAction("Login");
                }

                // Tìm user trong database
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == username);

                if (user == null)
                {
                    // Tạo user mới nếu không tìm thấy (fallback)
                    user = new User
                    {
                        IdUsers = "",
                        Username = username,
                        Email = HttpContext.Session.GetString("Email") ?? "",
                        Tennguoidung = HttpContext.Session.GetString("DisplayName") ?? username,
                        ImgUrl = "user.png",
                        PasswordUsers = "",
                        Roles = HttpContext.Session.GetString("VaiTro") ?? "",
                        Phone = "",
                        Diachi = "",
                        CreateAt = DateTime.Now
                    };
                }

                return View("Profile", user);
            }
            catch (Exception ex)
            {
                // Log lỗi và redirect về login
                ViewBag.Error = $"Có lỗi xảy ra: {ex.Message}";
                return RedirectToAction("Login");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(User model, IFormFile avatar)
        {
            try
            {
                // Lấy username từ session
                var username = HttpContext.Session.GetString("TenDangNhap");

                if (string.IsNullOrEmpty(username))
                {
                    return RedirectToAction("Login");
                }

                // Tìm user hiện tại
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == username);

                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                // Cập nhật thông tin
                user.Tennguoidung = model.Tennguoidung;
                user.Email = model.Email;
                user.Phone = model.Phone;
                user.Diachi = model.Diachi;

                // Xử lý upload avatar
                if (avatar != null && avatar.Length > 0)
                {
                    var fileExtension = Path.GetExtension(avatar.FileName);
                    var fileName = Guid.NewGuid().ToString("N")[..10] + fileExtension;

                    // Lưu file vào thư mục ImgUser
                    var uploadPath = Path.Combine("wwwroot/ImgUser", fileName);
                    using (var stream = new FileStream(uploadPath, FileMode.Create))
                    {
                        await avatar.CopyToAsync(stream);
                    }

                    // Lưu tên file vào database
                    user.ImgUrl = fileName;
                }

                // Cập nhật database
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                // Cập nhật session nếu tên hiển thị thay đổi
                string displayName = !string.IsNullOrEmpty(user.Tennguoidung) ? user.Tennguoidung : user.Username;
                HttpContext.Session.SetString("DisplayName", displayName ?? "");
                HttpContext.Session.SetString("Email", user.Email ?? "");

                ViewBag.Success = "Cập nhật thông tin thành công!";
                return View("Profile", user);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Có lỗi xảy ra: {ex.Message}";
                var username = HttpContext.Session.GetString("TenDangNhap");
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                return View("Profile", user);
            }
        }



        // API để kiểm tra email đã tồn tại
        [HttpPost]
        public async Task<IActionResult> CheckEmailExists(string email)
        {
            var username = HttpContext.Session.GetString("TenDangNhap");
            var exists = await _context.Users
                .AnyAsync(u => u.Email == email && u.Username != username);

            return Json(new { exists = exists });
        }



        [HttpGet]
        public IActionResult ChangePassword()
        {
            // Kiểm tra đăng nhập
            var username = HttpContext.Session.GetString("TenDangNhap");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            try
            {
                // Kiểm tra đăng nhập
                var username = HttpContext.Session.GetString("TenDangNhap");
                if (string.IsNullOrEmpty(username))
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập!" });
                }

                // Kiểm tra input
                if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
                {
                    return Json(new { success = false, message = "Vui lòng điền đầy đủ thông tin!" });
                }

                // Kiểm tra mật khẩu mới và xác nhận
                if (newPassword != confirmPassword)
                {
                    return Json(new { success = false, message = "Mật khẩu mới và xác nhận mật khẩu không khớp!" });
                }

                // Kiểm tra độ dài mật khẩu mới
                if (newPassword.Length < 8)
                {
                    return Json(new { success = false, message = "Mật khẩu mới phải có ít nhất 8 ký tự!" });
                }

                // Tìm user hiện tại
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == username);

                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy thông tin người dùng!" });
                }

                // Kiểm tra mật khẩu hiện tại
                if (user.PasswordUsers != currentPassword)
                {
                    return Json(new { success = false, message = "Mật khẩu hiện tại không đúng!" });
                }

                // Kiểm tra mật khẩu mới không trùng với mật khẩu cũ
                if (currentPassword == newPassword)
                {
                    return Json(new { success = false, message = "Mật khẩu mới phải khác mật khẩu hiện tại!" });
                }

                // Cập nhật mật khẩu mới
                user.PasswordUsers = newPassword;

                // Lưu vào database
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Đổi mật khẩu thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }

    }
}