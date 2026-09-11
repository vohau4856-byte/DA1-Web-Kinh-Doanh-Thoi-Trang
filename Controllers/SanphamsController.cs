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
    public class SanphamsController : Controller
    {
        private readonly ShopquanaoContext _context;

        public SanphamsController(ShopquanaoContext context)
        {
            _context = context;
        }

        // GET: Sanphams
        public async Task<IActionResult> Index()
        {
            var shopquanaoContext = _context.Sanphams.Include(s => s.IdDanhmucNavigation).Include(s => s.IdLoaiNavigation);
            return View(await shopquanaoContext.ToListAsync());
        }

        // GET: Sanphams/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanpham = await _context.Sanphams
                .Include(s => s.IdDanhmucNavigation)
                .Include(s => s.IdLoaiNavigation)
                .FirstOrDefaultAsync(m => m.IdSanpham == id);
            if (sanpham == null)
            {
                return NotFound();
            }

            return View(sanpham);
        }

        // GET: Sanphams/Create
        public IActionResult Create()
        {
            ViewData["IdDanhmuc"] = new SelectList(_context.Danhmucsanphams, "IdDanhmuc", "IdDanhmuc");
            ViewData["IdLoai"] = new SelectList(_context.Loaisanphams, "IdLoai", "IdLoai");
            return View();
        }

        // POST: Sanphams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSanpham,TenSanpham,Gia,SoLuongTonKho,IdDanhmuc,IdLoai,ImageUrl,Size,CreateAt,Sex")] Sanpham sanpham)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sanpham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDanhmuc"] = new SelectList(_context.Danhmucsanphams, "IdDanhmuc", "IdDanhmuc", sanpham.IdDanhmuc);
            ViewData["IdLoai"] = new SelectList(_context.Loaisanphams, "IdLoai", "IdLoai", sanpham.IdLoai);
            return View(sanpham);
        }

        // GET: Sanphams/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanpham = await _context.Sanphams.FindAsync(id);
            if (sanpham == null)
            {
                return NotFound();
            }
            ViewData["IdDanhmuc"] = new SelectList(_context.Danhmucsanphams, "IdDanhmuc", "IdDanhmuc", sanpham.IdDanhmuc);
            ViewData["IdLoai"] = new SelectList(_context.Loaisanphams, "IdLoai", "IdLoai", sanpham.IdLoai);
            return View(sanpham);
        }

        // POST: Sanphams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdSanpham,TenSanpham,Gia,SoLuongTonKho,IdDanhmuc,IdLoai,ImageUrl,Size,CreateAt,Sex")] Sanpham sanpham)
        {
            if (id != sanpham.IdSanpham)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sanpham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanphamExists(sanpham.IdSanpham))
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
            ViewData["IdDanhmuc"] = new SelectList(_context.Danhmucsanphams, "IdDanhmuc", "IdDanhmuc", sanpham.IdDanhmuc);
            ViewData["IdLoai"] = new SelectList(_context.Loaisanphams, "IdLoai", "IdLoai", sanpham.IdLoai);
            return View(sanpham);
        }

        // GET: Sanphams/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanpham = await _context.Sanphams
                .Include(s => s.IdDanhmucNavigation)
                .Include(s => s.IdLoaiNavigation)
                .FirstOrDefaultAsync(m => m.IdSanpham == id);
            if (sanpham == null)
            {
                return NotFound();
            }

            return View(sanpham);
        }

        // POST: Sanphams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var sanpham = await _context.Sanphams.FindAsync(id);
            if (sanpham != null)
            {
                _context.Sanphams.Remove(sanpham);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanphamExists(string id)
        {
            return _context.Sanphams.Any(e => e.IdSanpham == id);
        }
    }
}
