using System;
using System.Collections.Generic;

#nullable disable

namespace MovieTicketBooking.Models
{
    public partial class SedisteZaProekcija
    {
        public int IdSedisteZaProekcija { get; set; }
        public string Status { get; set; }
        public int Cena { get; set; }
        public int IdSediste { get; set; }
        public int IdProekcija { get; set; }
        public int? IdRezervacija { get; set; }

        public virtual Proekcija IdProekcijaNavigation { get; set; }
        public virtual Rezervacija IdRezervacijaNavigation { get; set; }
    }
}
