using System;
using System.Collections.Generic;

#nullable disable

namespace MovieTicketBooking.Models
{
    public partial class Korisnik
    {
        public Korisnik()
        {
            Plakanjes = new HashSet<Plakanje>();
            Rezervacijas = new HashSet<Rezervacija>();
        }

        public int IdKorisnik { get; set; }
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Plakanje> Plakanjes { get; set; }
        public virtual ICollection<Rezervacija> Rezervacijas { get; set; }
    }
}
