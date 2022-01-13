using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTicketBooking.Models
{
    public class PaymentModel
    {
        public int Suma { get; set; }
        [Display(Name = "Payment Method")]
        public string NacinNaPlakanje { get; set; }
        public DateTime DatumIVreme { get; set; }
        public int IdRezervacija { get; set; }
        public int IdKorisnik { get; set; }
        [Required]
        [Display(Name = "Name on Card")]
        public string NameOnCard { get; set; }
        [Required]
        [Display(Name = "Credit card number")]
        public string CCNumber { get; set; }
        [Required]
        [Display(Name = "Exp Month")]
        public int ExpMonth { get; set; }
        [Required]
        [Display(Name = "Exp Year")]
        public int ExpYear { get; set; }
        [Required]
        public string CVV { get; set; }
    }
}
