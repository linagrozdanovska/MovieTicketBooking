using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MovieTicketBooking.Services
{
    public class PendingStatusRemovalService : IHostedService, IDisposable
    {
        private Timer timer;
        private int number;

        private readonly postgresContext _context;

        public PendingStatusRemovalService(postgresContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(
                RemoveStatusPending,
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(10)
            );

            return Task.CompletedTask;
        }

        private void RemoveStatusPending(object state)
        {
            var reservations = _context.Rezervacijas
                .Include(r => r.IdKorisnikNavigation)
                .Include(r => r.IdProekcijaNavigation)
                .Include(r => r.IdProekcijaNavigation.IdFilmNavigation)
                .Where(r => r.Status.Equals("pending"))
                .ToList();

            var seats = _context.SedisteZaProekcijas
                .Include(s => s.IdProekcijaNavigation)
                .Include(s => s.IdRezervacijaNavigation)
                .Where(s => s.Status.Equals("pending"))
                .ToList();


            foreach (var r in reservations)
            {
                TimeSpan difference = r.DatumIVreme - DateTime.Now;
                double minutes = difference.Minutes;
                if (minutes >= 10)
                {
                    r.Status = "cancelled";
                     _context.Update(r);

                    foreach (var s in seats)
                    {
                        if (s.IdRezervacija == r.IdRezervacija)
                        {
                            s.Status = "available";
                            s.IdRezervacija = null;
                            _context.Update(s);
                        }
                    }
                }
                
            }

            _context.SaveChanges();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

    }
}
