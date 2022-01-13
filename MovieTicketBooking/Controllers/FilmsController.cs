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
            //RemoveStatusPending();
            return View(await _context.Films.ToListAsync());
        }

        private async void RemoveStatusPending()
        {
            var reservations = _context.Rezervacijas
                .Include(r => r.IdKorisnikNavigation)
                .Include(r => r.IdProekcijaNavigation)
                .Include(r => r.IdProekcijaNavigation.IdFilmNavigation)
                .Where(r => r.Status.Equals("pending"))
                .ToList();

            var seats = _context.SedisteZaProekcijas
                .Include(s => s.IdProekcijaNavigation)
                .Include(s => s.IdRezervacijaNavigation)
                .Where(s => s.Status.Equals("pending"))
                .ToList();


            foreach (var r in reservations)
            {
                TimeSpan difference = r.DatumIVreme - DateTime.Now;
                double minutes = difference.Minutes;
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

        // remove?
        // GET: Films/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films
                .FirstOrDefaultAsync(m => m.IdFilm == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }
    }
}
