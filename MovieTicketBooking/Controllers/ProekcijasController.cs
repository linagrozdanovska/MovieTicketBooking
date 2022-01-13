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
                .Where(p => p.IdFilm == id)
                .OrderByDescending(p => p.Datum);
            var movie = "";
            foreach(var p in postgresContext)
            {
                movie = p.IdFilmNavigation.Naslov;
            }
            ViewData["Movie"] = movie;
            return View(await postgresContext.ToListAsync());
        }
    }
}
