using System;
using System.Collections.Generic;

#nullable disable

namespace MovieTicketBooking.Models
{
    public partial class Proekcija
    {
        public Proekcija()
        {
            Rezervacijas = new HashSet<Rezervacija>();
            SedisteZaProekcijas = new HashSet<SedisteZaProekcija>();
        }

        public int IdProekcija { get; set; }
        public DateTime Datum { get; set; }
        public TimeSpan Vreme { get; set; }
        public string Tip { get; set; }
        public int IdFilm { get; set; }
        public int IdSala { get; set; }

        public virtual Film IdFilmNavigation { get; set; }
        public virtual Sala IdSalaNavigation { get; set; }
        public virtual ICollection<Rezervacija> Rezervacijas { get; set; }
        public virtual ICollection<SedisteZaProekcija> SedisteZaProekcijas { get; set; }
    }
}
