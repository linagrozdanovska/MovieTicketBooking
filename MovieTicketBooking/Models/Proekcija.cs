using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime Datum { get; set; }
        [Display(Name = "Time")]
        public TimeSpan Vreme { get; set; }
        [Display(Name = "Type")]
        public string Tip { get; set; }
        public int IdFilm { get; set; }
        public int IdSala { get; set; }

        public virtual Film IdFilmNavigation { get; set; }
        public virtual Sala IdSalaNavigation { get; set; }
        public virtual ICollection<Rezervacija> Rezervacijas { get; set; }
        public virtual ICollection<SedisteZaProekcija> SedisteZaProekcijas { get; set; }
    }
}
