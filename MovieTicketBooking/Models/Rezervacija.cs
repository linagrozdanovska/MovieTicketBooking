using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MovieTicketBooking.Models
{
    public partial class Rezervacija
    {
        public Rezervacija()
        {
            Plakanjes = new HashSet<Plakanje>();
            SedisteZaProekcijas = new HashSet<SedisteZaProekcija>();
        }

        public int IdRezervacija { get; set; }
        [Display(Name = "Seats")]
        public int BrojNaSedista { get; set; }
        public string Status { get; set; }
        [Display(Name = "Date & Time")]
        public DateTime DatumIVreme { get; set; }
        public int IdProekcija { get; set; }
        public int IdKorisnik { get; set; }

        public virtual Korisnik IdKorisnikNavigation { get; set; }
        public virtual Proekcija IdProekcijaNavigation { get; set; }
        public virtual ICollection<Plakanje> Plakanjes { get; set; }
        public virtual ICollection<SedisteZaProekcija> SedisteZaProekcijas { get; set; }
    }
}
