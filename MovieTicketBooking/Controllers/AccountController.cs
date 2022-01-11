using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTicketBooking.Controllers
{
    public class AccountController : Controller
    {
        private readonly postgresContext _context;

        public AccountController(postgresContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            RegisterModel model = new RegisterModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel Input)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Korisniks.Where(z => z.Email == Input.Email).FirstOrDefault();
                if (user != null)
                {
                    ModelState.AddModelError(string.Empty, "User alrready exists!");
                }
                else
                {
                    user = new Korisnik { KorisnickoIme = Input.Username, Lozinka = Input.Password, Email = Input.Email };
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Films");
                }
            }
            return RedirectToAction("Index", "Films");
        }
    }
}
