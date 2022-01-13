using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Controllers
{
    public class RezervacijasController : Controller
    {
        private readonly postgresContext _context;

        public RezervacijasController(postgresContext context)
        {
            _context = context;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeReservation(SelectedSeatsModel model)
        {
            DateTime date = DateTime.Now;
            List<SedisteZaProekcija> selectedSeats = new List<SedisteZaProekcija>();

            if (model.SelectedSeats is not null && model.SelectedSeats.Length > 0)
            {
                var postgresContext = _context.SedisteZaProekcijas
                .Include(s => s.IdProekcijaNavigation)
                .Include(s => s.IdRezervacijaNavigation)
                .OrderBy(p => p.IdSedisteZaProekcija);
                var allSeats = await postgresContext.ToListAsync();

                foreach (var seat in allSeats)
                {
                    if (model.SelectedSeats.Contains(seat.IdSedisteZaProekcija))
                    {
                        selectedSeats.Add(seat);
                    }
                }

                int userId = int.Parse(User.Claims.ToList()[0].Value);

                Rezervacija rezervacija = new Rezervacija
                {
                    BrojNaSedista = selectedSeats.Count,
                    Status = "pending",
                    DatumIVreme = date,
                    IdProekcija = model.IdProekcija,
                    IdKorisnik = userId
                };

                _context.Add(rezervacija);
                await _context.SaveChangesAsync();

            }
            else
            {
                return RedirectToAction("Index", "SedisteZaProekcijas", new { id = model.IdProekcija });
            }

            var res = _context.Rezervacijas
                .Include(r => r.IdKorisnikNavigation)
                .Include(r => r.IdProekcijaNavigation)
                .Include(r => r.IdProekcijaNavigation.IdFilmNavigation)
                .Where(r => r.DatumIVreme == date)
                .FirstOrDefault();

            foreach (var seat in selectedSeats)
            {
                seat.Status = "pending";
                seat.IdRezervacija = res.IdRezervacija;
                _context.Update(seat);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Rezervacijas", new { id =  res.IdRezervacija });
        }

        public async Task<IActionResult> Cancel(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacijas
                .Include(r => r.IdKorisnikNavigation)
                .Include(r => r.IdProekcijaNavigation)
                .Include(r => r.IdProekcijaNavigation.IdFilmNavigation)
                .FirstOrDefaultAsync(m => m.IdRezervacija == id);

            if (rezervacija == null)
            {
                return NotFound();
            }

            var postgresContext = _context.SedisteZaProekcijas
               .Include(s => s.IdProekcijaNavigation)
               .Include(s => s.IdRezervacijaNavigation)
               .Where(s => s.IdRezervacija == id)
               .OrderBy(p => p.IdSedisteZaProekcija);
            var seats = await postgresContext.ToListAsync();

            foreach (var seat in seats)
            {
                seat.Status = "available";
                seat.IdRezervacija = null;
                _context.Update(seat);
                await _context.SaveChangesAsync();
            }

            rezervacija.Status = "cancelled";
            _context.Update(rezervacija);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Rezervacijas", new { id = int.Parse(User.Claims.ToList()[0].Value) });
        }

        
        // GET: Rezervacijas
        public async Task<IActionResult> Index(int id)
        {
            var postgresContext = _context.Rezervacijas
                .Include(r => r.IdKorisnikNavigation)
                .Include(r => r.IdProekcijaNavigation)
                .Include(r => r.IdProekcijaNavigation.IdFilmNavigation)
                .Where(r => r.IdKorisnik == id)
                .OrderByDescending(r => r.DatumIVreme);
            return View(await postgresContext.ToListAsync());
        }

        // GET: Rezervacijas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacijas
                .Include(r => r.IdKorisnikNavigation)
                .Include(r => r.IdProekcijaNavigation)
                .Include(r => r.IdProekcijaNavigation.IdFilmNavigation)
                .FirstOrDefaultAsync(m => m.IdRezervacija == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        // GET: Rezervacijas/Create
        public IActionResult Create()
        {
            ViewData["IdKorisnik"] = new SelectList(_context.Korisniks, "IdKorisnik", "Email");
            ViewData["IdProekcija"] = new SelectList(_context.Proekcijas, "IdProekcija", "Tip");
            return View();
        }

        // POST: Rezervacijas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRezervacija,BrojNaSedista,Status,DatumIVreme,IdProekcija,IdKorisnik")] Rezervacija rezervacija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rezervacija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdKorisnik"] = new SelectList(_context.Korisniks, "IdKorisnik", "Email", rezervacija.IdKorisnik);
            ViewData["IdProekcija"] = new SelectList(_context.Proekcijas, "IdProekcija", "Tip", rezervacija.IdProekcija);
            return View(rezervacija);
        }

        // GET: Rezervacijas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacijas.FindAsync(id);
            if (rezervacija == null)
            {
                return NotFound();
            }
            ViewData["IdKorisnik"] = new SelectList(_context.Korisniks, "IdKorisnik", "Email", rezervacija.IdKorisnik);
            ViewData["IdProekcija"] = new SelectList(_context.Proekcijas, "IdProekcija", "Tip", rezervacija.IdProekcija);
            return View(rezervacija);
        }

        // POST: Rezervacijas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRezervacija,BrojNaSedista,Status,DatumIVreme,IdProekcija,IdKorisnik")] Rezervacija rezervacija)
        {
            if (id != rezervacija.IdRezervacija)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rezervacija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RezervacijaExists(rezervacija.IdRezervacija))
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
            ViewData["IdKorisnik"] = new SelectList(_context.Korisniks, "IdKorisnik", "Email", rezervacija.IdKorisnik);
            ViewData["IdProekcija"] = new SelectList(_context.Proekcijas, "IdProekcija", "Tip", rezervacija.IdProekcija);
            return View(rezervacija);
        }

        // GET: Rezervacijas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacijas
                .Include(r => r.IdKorisnikNavigation)
                .Include(r => r.IdProekcijaNavigation)
                .FirstOrDefaultAsync(m => m.IdRezervacija == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        // POST: Rezervacijas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervacija = await _context.Rezervacijas.FindAsync(id);
            _context.Rezervacijas.Remove(rezervacija);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RezervacijaExists(int id)
        {
            return _context.Rezervacijas.Any(e => e.IdRezervacija == id);
        }
    }
}
