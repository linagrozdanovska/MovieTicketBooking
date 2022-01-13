using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MovieTicketBooking.Models
{
    public partial class Plakanje
    {
        public int IdPlakanje { get; set; }
        public int Suma { get; set; }
        [Display(Name = "Payment Method")]
        public string NacinNaPlakanje { get; set; }
        public DateTime DatumIVreme { get; set; }
        public int IdRezervacija { get; set; }
        public int IdKorisnik { get; set; }

        public virtual Korisnik IdKorisnikNavigation { get; set; }
        public virtual Rezervacija IdRezervacijaNavigation { get; set; }
    }
}
