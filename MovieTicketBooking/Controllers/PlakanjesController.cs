using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Controllers
{
    public class PlakanjesController : Controller
    {
        private readonly postgresContext _context;

        public PlakanjesController(postgresContext context)
        {
            _context = context;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create(int? id)
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
            var showtimeSeats = await postgresContext.ToListAsync();
            var total = 0;
            foreach (var seat in showtimeSeats)
            {
                total += seat.Cena;
            }

            ViewData["Suma"] = total;
            ViewData["DatumIVreme"] = DateTime.Now;
            ViewData["IdRezervacija"] = rezervacija.IdRezervacija;
            ViewData["IdKorisnik"] = int.Parse(User.Claims.ToList()[0].Value);

            ViewData["Seats"] = showtimeSeats.Count;
            ViewData["Movie"] = rezervacija.IdProekcijaNavigation.IdFilmNavigation.Naslov;

            return View();
        }

        // POST: Plakanjes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentModel plakanje)
        {
            if (ModelState.IsValid)
            {
                Plakanje p = new Plakanje
                {
                    Suma = plakanje.Suma,
                    NacinNaPlakanje = plakanje.NacinNaPlakanje,
                    DatumIVreme = plakanje.DatumIVreme,
                    IdRezervacija = plakanje.IdRezervacija,
                    IdKorisnik = plakanje.IdKorisnik
                };

                _context.Add(p);

                var r = _context.Rezervacijas.ToList().Find(z => z.IdRezervacija == p.IdRezervacija);
                r.Status = "completed";

                var seats = await _context.SedisteZaProekcijas
               .Include(s => s.IdProekcijaNavigation)
               .Include(s => s.IdRezervacijaNavigation)
               .Where(s => s.IdRezervacija == r.IdRezervacija)
               .OrderBy(p => p.IdSedisteZaProekcija)
               .ToListAsync();

                foreach (var seat in seats)
                {
                    seat.Status = "reserved";
                    _context.Update(seat);
                    await _context.SaveChangesAsync();
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Rezervacijas", new { id = int.Parse(User.Claims.ToList()[0].Value) });
            }

            var rezervacija = await _context.Rezervacijas
                .Include(r => r.IdKorisnikNavigation)
                .Include(r => r.IdProekcijaNavigation)
                .Include(r => r.IdProekcijaNavigation.IdFilmNavigation)
                .FirstOrDefaultAsync(m => m.IdRezervacija == plakanje.IdRezervacija);

            if (rezervacija == null)
            {
                return NotFound();
            }

            var postgresContext = _context.SedisteZaProekcijas
               .Include(s => s.IdProekcijaNavigation)
               .Include(s => s.IdRezervacijaNavigation)
               .Where(s => s.IdRezervacija == plakanje.IdRezervacija)
               .OrderBy(p => p.IdSedisteZaProekcija);
            var showtimeSeats = await postgresContext.ToListAsync();
            var total = 0;
            foreach (var seat in showtimeSeats)
            {
                total += seat.Cena;
            }

            ViewData["Suma"] = total;
            ViewData["DatumIVreme"] = DateTime.Now;
            ViewData["IdRezervacija"] = rezervacija.IdRezervacija;
            ViewData["IdKorisnik"] = int.Parse(User.Claims.ToList()[0].Value);
            return View(plakanje);
        }
    }
}

