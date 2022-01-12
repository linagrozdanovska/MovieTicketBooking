using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTicketBooking.Models
{
    public class SelectedSeatsModel
    {
        public int IdProekcija { get; set; }
        public List<SedisteZaProekcija> AllSeats { get; set; }
        public int[] SelectedSeats { get; set; }
    }
}
