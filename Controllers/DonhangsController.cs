using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DA1CNTT.Models;

namespace DA1CNTT.Controllers
{
    public class DonhangsController : Controller
    {
        private readonly ShopquanaoContext _context;

        public DonhangsController(ShopquanaoContext context)
        {
            _context = context;
        }

        // GET: Donhangs
        public async Task<IActionResult> Index()
        {
            var shopquanaoContext = _context.Donhangs.Include(d => d.IdUsersNavigation);
            return View(await shopquanaoContext.ToListAsync());
        }

        // GET: Donhangs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donhang = await _context.Donhangs
                .Include(d => d.IdUsersNavigation)
                .FirstOrDefaultAsync(m => m.IdDonhang == id);
            if (donhang == null)
            {
                return NotFound();
            }

            return View(donhang);
        }

        // GET: Donhangs/Create
        public IActionResult Create()
        {
            ViewData["IdUsers"] = new SelectList(_context.Users, "IdUsers", "IdUsers");
            return View();
        }

        // POST: Donhangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDonhang,IdUsers,Ngaydathang,Trangthai")] Donhang donhang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donhang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdUsers"] = new SelectList(_context.Users, "IdUsers", "IdUsers", donhang.IdUsers);
            return View(donhang);
        }

        // GET: Donhangs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donhang = await _context.Donhangs.FindAsync(id);
            if (donhang == null)
            {
                return NotFound();
            }
            ViewData["IdUsers"] = new SelectList(_context.Users, "IdUsers", "IdUsers", donhang.IdUsers);
            return View(donhang);
        }

        // POST: Donhangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdDonhang,IdUsers,Ngaydathang,Trangthai")] Donhang donhang)
        {
            if (id != donhang.IdDonhang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donhang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonHangExists(donhang.IdDonhang))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdUsers"] = new SelectList(_context.Users, "IdUsers", "IdUsers", donhang.IdUsers);
            return View(donhang);
        }

        // GET: Donhangs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donhang = await _context.Donhangs
                .Include(d => d.IdUsersNavigation)
                .FirstOrDefaultAsync(m => m.IdDonhang == id);
            if (donhang == null)
            {
                return NotFound();
            }

            return View(donhang);
        }

        // POST: Donhangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var donhang = await _context.Donhangs.FindAsync(id);
            if (donhang != null)
            {
                _context.Donhangs.Remove(donhang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonHangExists(string id)
        {
            return _context.Donhangs.Any(e => e.IdDonhang == id);
        }





        [HttpGet]
        public IActionResult OrderSuccessful(string id)
        {
            var donHang = _context.Donhangs
                .Include(d => d.Chitietdonhangs)
                .ThenInclude(ct => ct.IdSanphamNavigation)
                .FirstOrDefault(d => d.IdDonhang == id);

            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }


        public async Task<IActionResult> DetailsWithProducts(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donhang = await _context.Donhangs
                .Include(d => d.IdUsersNavigation) // thông tin người dùng
                .Include(d => d.Chitietdonhangs)   // chi tiết đơn hàng
                    .ThenInclude(ct => ct.IdSanphamNavigation) // sản phẩm
                .FirstOrDefaultAsync(m => m.IdDonhang == id);

            if (donhang == null)
            {
                return NotFound();
            }

            return View(donhang); // tạo View mới DetailsWithProducts.cshtml
        }

    }
}



