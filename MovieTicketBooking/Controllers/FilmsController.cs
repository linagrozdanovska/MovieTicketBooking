using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Controllers
{
    public class FilmsController : Controller
    {
        private readonly postgresContext _context;

        public FilmsController(postgresContext context)
        {
            _context = context;
        }

        // GET: Films
        public async Task<IActionResult> Index()
        {
            await RemoveStatusPendingAsync();
            return View(await _context.Films.OrderByDescending(f => f.DatumNaIzdavanje).ToListAsync());
        }

        private async Task RemoveStatusPendingAsync()
        {
            var postgresContext1 = _context.Rezervacijas
                .Include(r => r.IdKorisnikNavigation)
                .Include(r => r.IdProekcijaNavigation)
                .Include(r => r.IdProekcijaNavigation.IdFilmNavigation)
                .Where(r => r.Status.Equals("pending"));
            var reservations = postgresContext1.ToList();

            var postgresContext2 = _context.SedisteZaProekcijas
                .Include(s => s.IdProekcijaNavigation)
                .Include(s => s.IdRezervacijaNavigation)
                .Where(s => s.Status.Equals("pending"));
            var seats = postgresContext2.ToList();


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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
