using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Controllers
{
    public class SedisteZaProekcijasController : Controller
    {
        private readonly postgresContext _context;

        public SedisteZaProekcijasController(postgresContext context)
        {
            _context = context;
        }

        // GET: SedisteZaProekcijas
        public async Task<IActionResult> Index(int id)
        {
            var postgresContext = _context.SedisteZaProekcijas
                .Include(s => s.IdProekcijaNavigation)
                .Include(s => s.IdRezervacijaNavigation)
                .Include(s => s.IdProekcijaNavigation.IdFilmNavigation)
                .Where(p => p.IdProekcija == id)
                .OrderBy(p => p.IdSedisteZaProekcija);
            var allSeats = await postgresContext.ToListAsync();
            var movie = "";
            foreach (var s in allSeats)
            {
                movie = s.IdProekcijaNavigation.IdFilmNavigation.Naslov;
            }
            ViewData["Movie"] = movie;
            return View(new SelectedSeatsModel
            {
                AllSeats = allSeats
            });
        }

        // GET: SedisteZaProekcijas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sedisteZaProekcija = await _context.SedisteZaProekcijas
                .Include(s => s.IdProekcijaNavigation)
                .Include(s => s.IdRezervacijaNavigation)
                .FirstOrDefaultAsync(m => m.IdSedisteZaProekcija == id);
            if (sedisteZaProekcija == null)
            {
                return NotFound();
            }

            return View(sedisteZaProekcija);
        }

        // GET: SedisteZaProekcijas/Create
        public IActionResult Create()
        {
            ViewData["IdProekcija"] = new SelectList(_context.Proekcijas, "IdProekcija", "Tip");
            ViewData["IdRezervacija"] = new SelectList(_context.Rezervacijas, "IdRezervacija", "Status");
            return View();
        }

        // POST: SedisteZaProekcijas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSedisteZaProekcija,Status,Cena,IdSediste,IdProekcija,IdRezervacija")] SedisteZaProekcija sedisteZaProekcija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sedisteZaProekcija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdProekcija"] = new SelectList(_context.Proekcijas, "IdProekcija", "Tip", sedisteZaProekcija.IdProekcija);
            ViewData["IdRezervacija"] = new SelectList(_context.Rezervacijas, "IdRezervacija", "Status", sedisteZaProekcija.IdRezervacija);
            return View(sedisteZaProekcija);
        }

        // GET: SedisteZaProekcijas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sedisteZaProekcija = await _context.SedisteZaProekcijas.FindAsync(id);
            if (sedisteZaProekcija == null)
            {
                return NotFound();
            }
            ViewData["IdProekcija"] = new SelectList(_context.Proekcijas, "IdProekcija", "Tip", sedisteZaProekcija.IdProekcija);
            ViewData["IdRezervacija"] = new SelectList(_context.Rezervacijas, "IdRezervacija", "Status", sedisteZaProekcija.IdRezervacija);
            return View(sedisteZaProekcija);
        }

        // POST: SedisteZaProekcijas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSedisteZaProekcija,Status,Cena,IdSediste,IdProekcija,IdRezervacija")] SedisteZaProekcija sedisteZaProekcija)
        {
            if (id != sedisteZaProekcija.IdSedisteZaProekcija)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sedisteZaProekcija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SedisteZaProekcijaExists(sedisteZaProekcija.IdSedisteZaProekcija))
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
            ViewData["IdProekcija"] = new SelectList(_context.Proekcijas, "IdProekcija", "Tip", sedisteZaProekcija.IdProekcija);
            ViewData["IdRezervacija"] = new SelectList(_context.Rezervacijas, "IdRezervacija", "Status", sedisteZaProekcija.IdRezervacija);
            return View(sedisteZaProekcija);
        }

        // GET: SedisteZaProekcijas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sedisteZaProekcija = await _context.SedisteZaProekcijas
                .Include(s => s.IdProekcijaNavigation)
                .Include(s => s.IdRezervacijaNavigation)
                .FirstOrDefaultAsync(m => m.IdSedisteZaProekcija == id);
            if (sedisteZaProekcija == null)
            {
                return NotFound();
            }

            return View(sedisteZaProekcija);
        }

        // POST: SedisteZaProekcijas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sedisteZaProekcija = await _context.SedisteZaProekcijas.FindAsync(id);
            _context.SedisteZaProekcijas.Remove(sedisteZaProekcija);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SedisteZaProekcijaExists(int id)
        {
            return _context.SedisteZaProekcijas.Any(e => e.IdSedisteZaProekcija == id);
        }
    }
}
