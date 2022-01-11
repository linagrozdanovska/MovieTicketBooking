using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        public string ReturnUrl { get; set; }

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
                    return RedirectToAction("Login", "Account");
                }
            }
            return RedirectToAction("Register", "Account");
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            LoginModel model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel Input)
        {
            ReturnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = _context.Korisniks.Where(z => z.KorisnickoIme == Input.Username && z.Lozinka == Input.Password).FirstOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid email or password!");
                }
                else
                {
                    var claims = new List<Claim>
                    {
                    new Claim(ClaimTypes.NameIdentifier, user.IdKorisnik.ToString()),
                    new Claim(ClaimTypes.Name, user.KorisnickoIme)
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            principal,
                            new AuthenticationProperties { IsPersistent = true });
                    return LocalRedirect(ReturnUrl);
                }
            }
            return RedirectToAction("Login", "Account");
        }

        public async Task Logout(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ReturnUrl = returnUrl;
        }
    }
}
