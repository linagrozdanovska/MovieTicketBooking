using System;
using System.Collections.Generic;

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
        public string Ime { get; set; }
        public int BrojNaSedista { get; set; }

        public virtual ICollection<Proekcija> Proekcijas { get; set; }
        public virtual ICollection<Sediste> Sedistes { get; set; }
    }
}
