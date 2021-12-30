using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MovieTicketBooking.Models
{
    public partial class Film
    {
        public Film()
        {
            Proekcijas = new HashSet<Proekcija>();
        }
        public int IdFilm { get; set; }
        [Display(Name = "Title")]
        public string Naslov { get; set; }
        [Display(Name = "Synopsis")]
        public string Opis { get; set; }
        [Display(Name = "Genre")]
        public string Zanr { get; set; }
        [Display(Name = "Running Time")]
        public int Vremetraenje { get; set; }
        [Display(Name = "Language")]
        public string Jazik { get; set; }
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DatumNaIzdavanje { get; set; }
        [Display(Name = "Director")]
        public string Reziser { get; set; }
        [Display(Name = "Starring")]
        public string Glumci { get; set; }

        public virtual ICollection<Proekcija> Proekcijas { get; set; }
    }
}
