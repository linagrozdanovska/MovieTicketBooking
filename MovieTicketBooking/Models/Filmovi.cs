using System;
using System.Collections.Generic;

#nullable disable

namespace MovieTicketBooking.Models
{
    public partial class Filmovi
    {
        public int? IdFilm { get; set; }
        public string Naslov { get; set; }
        public string Opis { get; set; }
        public string Zanr { get; set; }
        public string Reziser { get; set; }
        public string Glumci { get; set; }
        public DateTime? DatumNaIzdavanje { get; set; }
        public int? Vremetraenje { get; set; }
        public string Jazik { get; set; }
    }
}
