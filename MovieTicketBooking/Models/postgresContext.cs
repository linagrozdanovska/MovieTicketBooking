using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MovieTicketBooking.Models
{
    public partial class postgresContext : DbContext
    {
        public postgresContext()
        {
        }

        public postgresContext(DbContextOptions<postgresContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Film> Films { get; set; }
        public virtual DbSet<Filmovi> Filmovis { get; set; }
        public virtual DbSet<Korisnik> Korisniks { get; set; }
        public virtual DbSet<Plakanje> Plakanjes { get; set; }
        public virtual DbSet<Proekcii> Proekciis { get; set; }
        public virtual DbSet<Proekcija> Proekcijas { get; set; }
        public virtual DbSet<Rezervacija> Rezervacijas { get; set; }
        public virtual DbSet<Sala> Salas { get; set; }
        public virtual DbSet<Sediste> Sedistes { get; set; }
        public virtual DbSet<SedisteZaProekcija> SedisteZaProekcijas { get; set; }
        public virtual DbSet<Sedista> Sedista { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("name=myDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("adminpack")
                .HasAnnotation("Relational:Collation", "Macedonian_North Macedonia.1251");

            modelBuilder.Entity<Film>(entity =>
            {
                entity.HasKey(e => e.IdFilm)
                    .HasName("pk_film");

                entity.ToTable("film", "kino");

                entity.Property(e => e.IdFilm).HasColumnName("id_film");

                entity.Property(e => e.DatumNaIzdavanje)
                    .HasColumnType("date")
                    .HasColumnName("datum_na_izdavanje");

                entity.Property(e => e.Glumci)
                    .IsRequired()
                    .HasMaxLength(512)
                    .HasColumnName("glumci");

                entity.Property(e => e.Jazik)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("jazik");

                entity.Property(e => e.Naslov)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("naslov");

                entity.Property(e => e.Opis)
                    .IsRequired()
                    .HasMaxLength(512)
                    .HasColumnName("opis");

                entity.Property(e => e.Reziser)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("reziser");

                entity.Property(e => e.Vremetraenje).HasColumnName("vremetraenje");

                entity.Property(e => e.Zanr)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("zanr");
            });

            modelBuilder.Entity<Filmovi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("filmovi", "kino");

                entity.Property(e => e.DatumNaIzdavanje)
                    .HasColumnType("date")
                    .HasColumnName("datum_na_izdavanje");

                entity.Property(e => e.Glumci)
                    .HasMaxLength(512)
                    .HasColumnName("glumci");

                entity.Property(e => e.IdFilm).HasColumnName("id_film");

                entity.Property(e => e.Jazik)
                    .HasMaxLength(32)
                    .HasColumnName("jazik");

                entity.Property(e => e.Naslov)
                    .HasMaxLength(256)
                    .HasColumnName("naslov");

                entity.Property(e => e.Opis)
                    .HasMaxLength(512)
                    .HasColumnName("opis");

                entity.Property(e => e.Reziser)
                    .HasMaxLength(128)
                    .HasColumnName("reziser");

                entity.Property(e => e.Vremetraenje).HasColumnName("vremetraenje");

                entity.Property(e => e.Zanr)
                    .HasMaxLength(64)
                    .HasColumnName("zanr");
            });

            modelBuilder.Entity<Korisnik>(entity =>
            {
                entity.HasKey(e => e.IdKorisnik)
                    .HasName("pk_korisnik");

                entity.ToTable("korisnik", "kino");

                entity.HasIndex(e => e.Email, "korisnik_email_key")
                    .IsUnique();

                entity.HasIndex(e => e.KorisnickoIme, "korisnik_korisnicko_ime_key")
                    .IsUnique();

                entity.Property(e => e.IdKorisnik).HasColumnName("id_korisnik");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("email");

                entity.Property(e => e.KorisnickoIme)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("korisnicko_ime");

                entity.Property(e => e.Lozinka)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("lozinka");
            });

            modelBuilder.Entity<Plakanje>(entity =>
            {
                entity.HasKey(e => e.IdPlakanje)
                    .HasName("pk_plakanje");

                entity.ToTable("plakanje", "kino");

                entity.Property(e => e.IdPlakanje).HasColumnName("id_plakanje");

                entity.Property(e => e.DatumIVreme).HasColumnName("datum_i_vreme");

                entity.Property(e => e.IdKorisnik).HasColumnName("id_korisnik");

                entity.Property(e => e.IdRezervacija).HasColumnName("id_rezervacija");

                entity.Property(e => e.NacinNaPlakanje)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("nacin_na_plakanje");

                entity.Property(e => e.Suma).HasColumnName("suma");

                entity.HasOne(d => d.IdKorisnikNavigation)
                    .WithMany(p => p.Plakanjes)
                    .HasForeignKey(d => d.IdKorisnik)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_plakanje_korisnik");

                entity.HasOne(d => d.IdRezervacijaNavigation)
                    .WithMany(p => p.Plakanjes)
                    .HasForeignKey(d => d.IdRezervacija)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_plakanje_rezervacija");
            });

            modelBuilder.Entity<Proekcii>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("proekcii", "kino");

                entity.Property(e => e.Datum)
                    .HasColumnType("date")
                    .HasColumnName("datum");

                entity.Property(e => e.IdFilm).HasColumnName("id_film");

                entity.Property(e => e.IdProekcija).HasColumnName("id_proekcija");

                entity.Property(e => e.IdSala).HasColumnName("id_sala");

                entity.Property(e => e.Ime)
                    .HasMaxLength(64)
                    .HasColumnName("ime");

                entity.Property(e => e.Naslov)
                    .HasMaxLength(256)
                    .HasColumnName("naslov");

                entity.Property(e => e.Tip)
                    .HasMaxLength(16)
                    .HasColumnName("tip");

                entity.Property(e => e.Vreme)
                    .HasColumnType("time without time zone")
                    .HasColumnName("vreme");
            });

            modelBuilder.Entity<Proekcija>(entity =>
            {
                entity.HasKey(e => e.IdProekcija)
                    .HasName("pk_proekcija");

                entity.ToTable("proekcija", "kino");

                entity.Property(e => e.IdProekcija).HasColumnName("id_proekcija");

                entity.Property(e => e.Datum)
                    .HasColumnType("date")
                    .HasColumnName("datum");

                entity.Property(e => e.IdFilm).HasColumnName("id_film");

                entity.Property(e => e.IdSala).HasColumnName("id_sala");

                entity.Property(e => e.Tip)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("tip");

                entity.Property(e => e.Vreme)
                    .HasColumnType("time without time zone")
                    .HasColumnName("vreme");

                entity.HasOne(d => d.IdFilmNavigation)
                    .WithMany(p => p.Proekcijas)
                    .HasForeignKey(d => d.IdFilm)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_proekcija_film");

                entity.HasOne(d => d.IdSalaNavigation)
                    .WithMany(p => p.Proekcijas)
                    .HasForeignKey(d => d.IdSala)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_proekcija_sala");
            });

            modelBuilder.Entity<Rezervacija>(entity =>
            {
                entity.HasKey(e => e.IdRezervacija)
                    .HasName("pk_rezervacija");

                entity.ToTable("rezervacija", "kino");

                entity.Property(e => e.IdRezervacija).HasColumnName("id_rezervacija");

                entity.Property(e => e.BrojNaSedista).HasColumnName("broj_na_sedista");

                entity.Property(e => e.DatumIVreme).HasColumnName("datum_i_vreme");

                entity.Property(e => e.IdKorisnik).HasColumnName("id_korisnik");

                entity.Property(e => e.IdProekcija).HasColumnName("id_proekcija");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("status");

                entity.HasOne(d => d.IdKorisnikNavigation)
                    .WithMany(p => p.Rezervacijas)
                    .HasForeignKey(d => d.IdKorisnik)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rezervacija_korisnik");

                entity.HasOne(d => d.IdProekcijaNavigation)
                    .WithMany(p => p.Rezervacijas)
                    .HasForeignKey(d => d.IdProekcija)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_rezervacija_proekcija");
            });

            modelBuilder.Entity<Sala>(entity =>
            {
                entity.HasKey(e => e.IdSala)
                    .HasName("pk_sala");

                entity.ToTable("sala", "kino");

                entity.Property(e => e.IdSala)
                    .ValueGeneratedNever()
                    .HasColumnName("id_sala");

                entity.Property(e => e.BrojNaSedista).HasColumnName("broj_na_sedista");

                entity.Property(e => e.Ime)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("ime");
            });

            modelBuilder.Entity<Sediste>(entity =>
            {
                entity.HasKey(e => e.IdSediste)
                    .HasName("pk_sediste");

                entity.ToTable("sediste", "kino");

                entity.Property(e => e.IdSediste).HasColumnName("id_sediste");

                entity.Property(e => e.Broj).HasColumnName("broj");

                entity.Property(e => e.IdSala).HasColumnName("id_sala");

                entity.Property(e => e.Tip)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("tip");

                entity.HasOne(d => d.IdSalaNavigation)
                    .WithMany(p => p.Sedistes)
                    .HasForeignKey(d => d.IdSala)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sediste_sala");
            });

            modelBuilder.Entity<SedisteZaProekcija>(entity =>
            {
                entity.HasKey(e => e.IdSedisteZaProekcija)
                    .HasName("pk_sediste_za_proekcija");

                entity.ToTable("sediste_za_proekcija", "kino");

                entity.Property(e => e.IdSedisteZaProekcija).HasColumnName("id_sediste_za_proekcija");

                entity.Property(e => e.Cena).HasColumnName("cena");

                entity.Property(e => e.IdProekcija).HasColumnName("id_proekcija");

                entity.Property(e => e.IdRezervacija).HasColumnName("id_rezervacija");

                entity.Property(e => e.IdSediste).HasColumnName("id_sediste");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("status");

                entity.HasOne(d => d.IdProekcijaNavigation)
                    .WithMany(p => p.SedisteZaProekcijas)
                    .HasForeignKey(d => d.IdProekcija)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_sediste_za_proekcija_proekcija");

                entity.HasOne(d => d.IdRezervacijaNavigation)
                    .WithMany(p => p.SedisteZaProekcijas)
                    .HasForeignKey(d => d.IdRezervacija)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_sediste_za_proekcija_rezervacija");
            });

            modelBuilder.Entity<Sedista>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("sedista", "kino");

                entity.Property(e => e.BrojNaSediste).HasColumnName("broj_na_sediste");

                entity.Property(e => e.Datum)
                    .HasColumnType("date")
                    .HasColumnName("datum");

                entity.Property(e => e.IdFilm).HasColumnName("id_film");

                entity.Property(e => e.IdProekcija).HasColumnName("id_proekcija");

                entity.Property(e => e.IdSala).HasColumnName("id_sala");

                entity.Property(e => e.IdSediste).HasColumnName("id_sediste");

                entity.Property(e => e.Ime)
                    .HasMaxLength(64)
                    .HasColumnName("ime");

                entity.Property(e => e.Naslov)
                    .HasMaxLength(256)
                    .HasColumnName("naslov");

                entity.Property(e => e.Status)
                    .HasMaxLength(16)
                    .HasColumnName("status");

                entity.Property(e => e.Tip)
                    .HasMaxLength(16)
                    .HasColumnName("tip");

                entity.Property(e => e.Vreme)
                    .HasColumnType("time without time zone")
                    .HasColumnName("vreme");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
