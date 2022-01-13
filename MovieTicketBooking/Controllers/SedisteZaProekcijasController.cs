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
    }
}
