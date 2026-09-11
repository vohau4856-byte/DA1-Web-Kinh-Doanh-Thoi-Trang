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
    public class ChitietdonhangsController : Controller
    {
        private readonly ShopquanaoContext _context;

        public ChitietdonhangsController(ShopquanaoContext context)
        {
            _context = context;
        }

        // GET: Chitietdonhangs
        public async Task<IActionResult> Index()
        {
            var shopquanaoContext = _context.Chitietdonhangs.Include(c => c.IdDonhangNavigation).Include(c => c.IdSanphamNavigation);
            return View(await shopquanaoContext.ToListAsync());
        }

        // GET: Chitietdonhangs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chitietdonhang = await _context.Chitietdonhangs
                .Include(c => c.IdDonhangNavigation)
                .Include(c => c.IdSanphamNavigation)
                .FirstOrDefaultAsync(m => m.IdOrder == id);
            if (chitietdonhang == null)
            {
                return NotFound();
            }

            return View(chitietdonhang);
        }

        // GET: Chitietdonhangs/Create
        public IActionResult Create()
        {
            ViewData["IdDonhang"] = new SelectList(_context.Donhangs, "IdDonhang", "IdDonhang");
            ViewData["IdSanpham"] = new SelectList(_context.Sanphams, "IdSanpham", "IdSanpham");
            return View();
        }

        // POST: Chitietdonhangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdOrder,IdDonhang,IdSanpham,Soluong,Giadat")] Chitietdonhang chitietdonhang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chitietdonhang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDonhang"] = new SelectList(_context.Donhangs, "IdDonhang", "IdDonhang", chitietdonhang.IdDonhang);
            ViewData["IdSanpham"] = new SelectList(_context.Sanphams, "IdSanpham", "IdSanpham", chitietdonhang.IdSanpham);
            return View(chitietdonhang);
        }

        // GET: Chitietdonhangs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chitietdonhang = await _context.Chitietdonhangs.FindAsync(id);
            if (chitietdonhang == null)
            {
                return NotFound();
            }
            ViewData["IdDonhang"] = new SelectList(_context.Donhangs, "IdDonhang", "IdDonhang", chitietdonhang.IdDonhang);
            ViewData["IdSanpham"] = new SelectList(_context.Sanphams, "IdSanpham", "IdSanpham", chitietdonhang.IdSanpham);
            return View(chitietdonhang);
        }

        // POST: Chitietdonhangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdOrder,IdDonhang,IdSanpham,Soluong,Giadat")] Chitietdonhang chitietdonhang)
        {
            if (id != chitietdonhang.IdOrder)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chitietdonhang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChitietdonhangExists(chitietdonhang.IdOrder))
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
            ViewData["IdDonhang"] = new SelectList(_context.Donhangs, "IdDonhang", "IdDonhang", chitietdonhang.IdDonhang);
            ViewData["IdSanpham"] = new SelectList(_context.Sanphams, "IdSanpham", "IdSanpham", chitietdonhang.IdSanpham);
            return View(chitietdonhang);
        }

        // GET: Chitietdonhangs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chitietdonhang = await _context.Chitietdonhangs
                .Include(c => c.IdDonhangNavigation)
                .Include(c => c.IdSanphamNavigation)
                .FirstOrDefaultAsync(m => m.IdOrder == id);
            if (chitietdonhang == null)
            {
                return NotFound();
            }

            return View(chitietdonhang);
        }

        // POST: Chitietdonhangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var chitietdonhang = await _context.Chitietdonhangs.FindAsync(id);
            if (chitietdonhang != null)
            {
                _context.Chitietdonhangs.Remove(chitietdonhang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChitietdonhangExists(string id)
        {
            return _context.Chitietdonhangs.Any(e => e.IdOrder == id);
        }
    }
}
