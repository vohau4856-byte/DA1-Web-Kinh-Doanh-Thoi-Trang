using Microsoft.AspNetCore.Mvc;
using DA1CNTT.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

public class SanphamController : Controller
{
    private readonly ShopquanaoContext _context;

    public SanphamController(ShopquanaoContext context)
    {
        _context = context;
    }

    // Phương thức loại bỏ dấu tiếng Việt
    private string RemoveVietnameseDiacritics(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        string normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    // Hiển thị tất cả sản phẩm với chức năng tìm kiếm
    public async Task<IActionResult> Index(string? danhmuc = null, string? sortBy = null, string? search = null, int page = 1,
    int pageSize = 12)
    {
        



        // Lấy danh sách danh mục để hiển thị sidebar
        var danhmucs = await _context.Danhmucsanphams.ToListAsync();
        ViewBag.Danhmucs = danhmucs;

        // Bắt đầu với query cơ bản
        var query = _context.Sanphams
            .Include(s => s.IdDanhmucNavigation)
            .Include(s => s.IdLoaiNavigation)
            .AsQueryable();

        // Lọc theo danh mục trước (có thể làm trong database)
        if (!string.IsNullOrEmpty(danhmuc))
        {
            query = query.Where(s => s.IdDanhmuc == danhmuc);
            ViewBag.SelectedCategory = danhmuc;
        }

        // Load dữ liệu từ database
        var allProducts = await query.ToListAsync();

        // Tìm kiếm theo từ khóa (xử lý trong memory với hỗ trợ không dấu)
        IEnumerable<DA1CNTT.Models.Sanpham> filteredProducts = allProducts;

        if (!string.IsNullOrEmpty(search))
        {
            string searchWithoutDiacritics = RemoveVietnameseDiacritics(search.ToLower());

            filteredProducts = allProducts.Where(s =>
                RemoveVietnameseDiacritics(s.TenSanpham?.ToLower() ?? "").Contains(searchWithoutDiacritics) ||
                RemoveVietnameseDiacritics(s.IdSanpham?.ToLower() ?? "").Contains(searchWithoutDiacritics) ||
                RemoveVietnameseDiacritics(s.IdDanhmucNavigation?.NameDanhmuc?.ToLower() ?? "").Contains(searchWithoutDiacritics) ||
                RemoveVietnameseDiacritics(s.IdLoaiNavigation?.NameLoai?.ToLower() ?? "").Contains(searchWithoutDiacritics)
            );

            ViewBag.SearchKeyword = search;
        }

        // Sắp xếp
        switch (sortBy)
        {
            case "newest":
                filteredProducts = filteredProducts.OrderByDescending(s => s.CreateAt);
                break;
            case "price_asc":
                filteredProducts = filteredProducts.OrderBy(s => s.Gia);
                break;
            case "price_desc":
                filteredProducts = filteredProducts.OrderByDescending(s => s.Gia);
                break;
            case "name_asc":
                filteredProducts = filteredProducts.OrderBy(s => s.TenSanpham);
                break;
            case "name_desc":
                filteredProducts = filteredProducts.OrderByDescending(s => s.TenSanpham);
                break;
            default:
                filteredProducts = filteredProducts.OrderBy(s => s.TenSanpham);
                break;
        }

        int totalProducts = filteredProducts.Count();

        var sanphams = filteredProducts
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToList();

        ViewBag.CurrentPage = page;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalProducts = totalProducts;
        ViewBag.TotalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
        ViewBag.SortBy = sortBy;

        return View(sanphams);
    }

    // API tìm kiếm AJAX với hỗ trợ không dấu
    [HttpGet]
    public async Task<IActionResult> Search(string keyword)
    {
        if (string.IsNullOrEmpty(keyword))
        {
            return Json(new { success = false, message = "Vui lòng nhập từ khóa tìm kiếm" });
        }

        // Chuyển từ khóa về không dấu
        string keywordWithoutDiacritics = RemoveVietnameseDiacritics(keyword.ToLower());

        // Lấy tất cả sản phẩm trước
        var allProducts = await _context.Sanphams
            .Include(s => s.IdDanhmucNavigation)
            .Include(s => s.IdLoaiNavigation)
            .ToListAsync();

        // Filter trong memory với hỗ trợ không dấu
        var sanphams = allProducts
            .Where(s =>
                RemoveVietnameseDiacritics(s.TenSanpham?.ToLower() ?? "").Contains(keywordWithoutDiacritics) ||
                RemoveVietnameseDiacritics(s.IdSanpham?.ToLower() ?? "").Contains(keywordWithoutDiacritics) ||
                RemoveVietnameseDiacritics(s.IdDanhmucNavigation?.NameDanhmuc?.ToLower() ?? "").Contains(keywordWithoutDiacritics) ||
                RemoveVietnameseDiacritics(s.IdLoaiNavigation?.NameLoai?.ToLower() ?? "").Contains(keywordWithoutDiacritics)
            )
            .Take(10) // Giới hạn 10 kết quả cho autocomplete
            .Select(s => new
            {
                id = s.IdSanpham,
                name = s.TenSanpham,
                price = s.Gia,
                image = s.ImageUrl,
                category = s.IdDanhmucNavigation?.NameDanhmuc
            })
            .ToList();

        return Json(new { success = true, data = sanphams });
    }

    // API để lấy sản phẩm theo danh mục (AJAX)
    [HttpGet]
    public async Task<IActionResult> GetByCategory(string categoryId)
    {
        var sanphams = await _context.Sanphams
            .Include(s => s.IdDanhmucNavigation)
            .Include(s => s.IdLoaiNavigation)
            .Where(s => s.IdDanhmuc == categoryId)
            .ToListAsync();

        return PartialView("_ProductGrid", sanphams);
    }

    // API tìm kiếm nâng cao với nhiều tùy chọn
    [HttpGet]
    public async Task<IActionResult> AdvancedSearch(string keyword, bool exactMatch = false, bool searchInDescription = true)
    {
        if (string.IsNullOrEmpty(keyword))
        {
            return Json(new { success = false, message = "Vui lòng nhập từ khóa tìm kiếm" });
        }

        var allProducts = await _context.Sanphams
            .Include(s => s.IdDanhmucNavigation)
            .Include(s => s.IdLoaiNavigation)
            .ToListAsync();

        IEnumerable<object> results;

        if (exactMatch)
        {
            // Tìm kiếm chính xác
            results = allProducts
                .Where(s =>
                    string.Equals(RemoveVietnameseDiacritics(s.TenSanpham?.ToLower() ?? ""),
                                 RemoveVietnameseDiacritics(keyword.ToLower()),
                                 StringComparison.OrdinalIgnoreCase) ||
                    (searchInDescription && string.Equals(RemoveVietnameseDiacritics(s.IdSanpham?.ToLower() ?? ""),
                                                         RemoveVietnameseDiacritics(keyword.ToLower()),
                                                         StringComparison.OrdinalIgnoreCase))
                )
                .Select(s => new
                {
                    id = s.IdSanpham,
                    name = s.TenSanpham,
                    price = s.Gia,
                    image = s.ImageUrl,
                    category = s.IdDanhmucNavigation?.NameDanhmuc,

                });
        }
        else
        {
            // Tìm kiếm gần đúng
            string keywordWithoutDiacritics = RemoveVietnameseDiacritics(keyword.ToLower());

            results = allProducts
                .Where(s =>
                    RemoveVietnameseDiacritics(s.TenSanpham?.ToLower() ?? "").Contains(keywordWithoutDiacritics) ||
                    (searchInDescription && RemoveVietnameseDiacritics(s.IdSanpham?.ToLower() ?? "").Contains(keywordWithoutDiacritics)) ||
                    RemoveVietnameseDiacritics(s.IdDanhmucNavigation?.NameDanhmuc?.ToLower() ?? "").Contains(keywordWithoutDiacritics) ||
                    RemoveVietnameseDiacritics(s.IdLoaiNavigation?.NameLoai?.ToLower() ?? "").Contains(keywordWithoutDiacritics)
                )
                .Select(s => new
                {
                    id = s.IdSanpham,
                    name = s.TenSanpham,
                    price = s.Gia,
                    image = s.ImageUrl,
                    category = s.IdDanhmucNavigation?.NameDanhmuc,

                });
        }

        return Json(new { success = true, data = results.Take(20).ToList(), total = results.Count() });
    }

    public async Task<IActionResult> DetailsSanPham(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound("Không tìm thấy sản phẩm");
        }

        var sanpham = await _context.Sanphams
            .Include(s => s.IdDanhmucNavigation)
            .Include(s => s.IdLoaiNavigation)
            .FirstOrDefaultAsync(s => s.IdSanpham == id);

        if (sanpham == null)
        {
            return NotFound("Sản phẩm không tồn tại");
        }

        // Lấy các sản phẩm liên quan (cùng danh mục, khác ID)
        var relatedProducts = await _context.Sanphams
            .Include(s => s.IdDanhmucNavigation)
            .Include(s => s.IdLoaiNavigation)
            .Where(s => s.IdDanhmuc == sanpham.IdDanhmuc && s.IdSanpham != id)
            .Take(4)
            .ToListAsync();

        ViewBag.RelatedProducts = relatedProducts;

        // Tạo breadcrumb
        ViewBag.CategoryName = sanpham.IdDanhmucNavigation?.NameDanhmuc ?? "Danh mục";
        ViewBag.ProductName = sanpham.TenSanpham;

        return View(sanpham);
    }


}