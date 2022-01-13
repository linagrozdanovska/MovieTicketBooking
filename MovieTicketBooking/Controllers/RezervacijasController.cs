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

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
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

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Index(int id)
        {
            RemoveStatusPending();
            var postgresContext = _context.Rezervacijas
                .Include(r => r.IdKorisnikNavigation)
                .Include(r => r.IdProekcijaNavigation)
                .Include(r => r.IdProekcijaNavigation.IdFilmNavigation)
                .Where(r => r.IdKorisnik == id)
                .OrderByDescending(r => r.DatumIVreme);
            return View(await postgresContext.ToListAsync());
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
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

        private async void RemoveStatusPending()
        {
            var postgresContext1 = _context.Rezervacijas
                .Include(r => r.IdKorisnikNavigation)
                .Include(r => r.IdProekcijaNavigation)
                .Include(r => r.IdProekcijaNavigation.IdFilmNavigation)
                .Where(r => r.Status.Equals("pending"));
            var reservations = await postgresContext1.ToListAsync();


            var postgresContext2 = _context.SedisteZaProekcijas
                .Include(s => s.IdProekcijaNavigation)
                .Include(s => s.IdRezervacijaNavigation)
                .Where(s => s.Status.Equals("pending"));
            var seats = await postgresContext2.ToListAsync();


            foreach (var r in reservations)
            {
                TimeSpan difference = DateTime.Now - r.DatumIVreme;
                double minutes = difference.TotalMinutes;
                if (minutes >= 10)
                {
                    r.Status = "cancelled";
                    _context.Update(r);

                    foreach (var s in seats)
                    {
                        if (s.IdRezervacija == r.IdRezervacija)
                        {
                            s.Status = "available";
                            s.IdRezervacija = null;
                            _context.Update(s);
                        }
                    }
                }

            }

            await _context.SaveChangesAsync();
        }
    }
}
