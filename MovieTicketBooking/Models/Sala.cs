using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MovieTicketBooking.Models
{
    public partial class Sala
    {
        public Sala()
        {
            Proekcijas = new HashSet<Proekcija>();
            Sedistes = new HashSet<Sediste>();
        }

        public int IdSala { get; set; }
        [Display(Name = "Hall")]
        public string Ime { get; set; }
        public int BrojNaSedista { get; set; }

        public virtual ICollection<Proekcija> Proekcijas { get; set; }
        public virtual ICollection<Sediste> Sedistes { get; set; }
    }
}
