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
    public class DanhmucsanphamsController : Controller
    {
        private readonly ShopquanaoContext _context;

        public DanhmucsanphamsController(ShopquanaoContext context)
        {
            _context = context;
        }

        // GET: Danhmucsanphams
        public async Task<IActionResult> Index()
        {
            return View(await _context.Danhmucsanphams.ToListAsync());
        }

        // GET: Danhmucsanphams/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhmucsanpham = await _context.Danhmucsanphams
                .FirstOrDefaultAsync(m => m.IdDanhmuc == id);
            if (danhmucsanpham == null)
            {
                return NotFound();
            }

            return View(danhmucsanpham);
        }

        // GET: Danhmucsanphams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Danhmucsanphams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDanhmuc,NameDanhmuc")] Danhmucsanpham danhmucsanpham)
        {
            if (ModelState.IsValid)
            {
                _context.Add(danhmucsanpham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(danhmucsanpham);
        }

        // GET: Danhmucsanphams/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhmucsanpham = await _context.Danhmucsanphams.FindAsync(id);
            if (danhmucsanpham == null)
            {
                return NotFound();
            }
            return View(danhmucsanpham);
        }

        // POST: Danhmucsanphams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdDanhmuc,NameDanhmuc")] Danhmucsanpham danhmucsanpham)
        {
            if (id != danhmucsanpham.IdDanhmuc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(danhmucsanpham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanhmucsanphamExists(danhmucsanpham.IdDanhmuc))
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
            return View(danhmucsanpham);
        }

        // GET: Danhmucsanphams/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhmucsanpham = await _context.Danhmucsanphams
                .FirstOrDefaultAsync(m => m.IdDanhmuc == id);
            if (danhmucsanpham == null)
            {
                return NotFound();
            }

            return View(danhmucsanpham);
        }

        // POST: Danhmucsanphams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var danhmucsanpham = await _context.Danhmucsanphams.FindAsync(id);
            if (danhmucsanpham != null)
            {
                _context.Danhmucsanphams.Remove(danhmucsanpham);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DanhmucsanphamExists(string id)
        {
            return _context.Danhmucsanphams.Any(e => e.IdDanhmuc == id);
        }
    }
}
