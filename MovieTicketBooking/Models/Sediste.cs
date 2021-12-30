using System;
using System.Collections.Generic;

#nullable disable

namespace MovieTicketBooking.Models
{
    public partial class Sediste
    {
        public int IdSediste { get; set; }
        public int Broj { get; set; }
        public string Tip { get; set; }
        public int IdSala { get; set; }

        public virtual Sala IdSalaNavigation { get; set; }
    }
}
