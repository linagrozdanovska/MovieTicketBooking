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
    public class ProekcijasController : Controller
    {
        private readonly postgresContext _context;

        public ProekcijasController(postgresContext context)
        {
            _context = context;
        }

        // GET: Proekcijas
        public async Task<IActionResult> Index(int id)
        {
            var postgresContext = _context.Proekcijas
                .Include(p => p.IdFilmNavigation)
                .Include(p => p.IdSalaNavigation)
                .Where(p => p.IdFilmNavigation.IdFilm == id);
            return View(await postgresContext.ToListAsync());
        }

        // GET: Proekcijas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proekcija = await _context.Proekcijas
                .Include(p => p.IdFilmNavigation)
                .Include(p => p.IdSalaNavigation)
                .FirstOrDefaultAsync(m => m.IdProekcija == id);
            if (proekcija == null)
            {
                return NotFound();
            }

            return View(proekcija);
        }

        // GET: Proekcijas/Create
        public IActionResult Create()
        {
            ViewData["IdFilm"] = new SelectList(_context.Films, "IdFilm", "Glumci");
            ViewData["IdSala"] = new SelectList(_context.Salas, "IdSala", "Ime");
            return View();
        }

        // POST: Proekcijas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProekcija,Datum,Vreme,Tip,IdFilm,IdSala")] Proekcija proekcija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proekcija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdFilm"] = new SelectList(_context.Films, "IdFilm", "Glumci", proekcija.IdFilm);
            ViewData["IdSala"] = new SelectList(_context.Salas, "IdSala", "Ime", proekcija.IdSala);
            return View(proekcija);
        }

        // GET: Proekcijas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proekcija = await _context.Proekcijas.FindAsync(id);
            if (proekcija == null)
            {
                return NotFound();
            }
            ViewData["IdFilm"] = new SelectList(_context.Films, "IdFilm", "Glumci", proekcija.IdFilm);
            ViewData["IdSala"] = new SelectList(_context.Salas, "IdSala", "Ime", proekcija.IdSala);
            return View(proekcija);
        }

        // POST: Proekcijas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProekcija,Datum,Vreme,Tip,IdFilm,IdSala")] Proekcija proekcija)
        {
            if (id != proekcija.IdProekcija)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proekcija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProekcijaExists(proekcija.IdProekcija))
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
            ViewData["IdFilm"] = new SelectList(_context.Films, "IdFilm", "Glumci", proekcija.IdFilm);
            ViewData["IdSala"] = new SelectList(_context.Salas, "IdSala", "Ime", proekcija.IdSala);
            return View(proekcija);
        }

        // GET: Proekcijas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proekcija = await _context.Proekcijas
                .Include(p => p.IdFilmNavigation)
                .Include(p => p.IdSalaNavigation)
                .FirstOrDefaultAsync(m => m.IdProekcija == id);
            if (proekcija == null)
            {
                return NotFound();
            }

            return View(proekcija);
        }

        // POST: Proekcijas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proekcija = await _context.Proekcijas.FindAsync(id);
            _context.Proekcijas.Remove(proekcija);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProekcijaExists(int id)
        {
            return _context.Proekcijas.Any(e => e.IdProekcija == id);
        }
    }
}
