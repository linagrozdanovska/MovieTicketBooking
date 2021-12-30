using System;
using System.Collections.Generic;

#nullable disable

namespace MovieTicketBooking.Models
{
    public partial class Proekcii
    {
        public int? IdFilm { get; set; }
        public string Naslov { get; set; }
        public int? IdProekcija { get; set; }
        public DateTime? Datum { get; set; }
        public TimeSpan? Vreme { get; set; }
        public string Tip { get; set; }
        public int? IdSala { get; set; }
        public string Ime { get; set; }
    }
}
