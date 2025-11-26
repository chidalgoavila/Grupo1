using Microsoft.EntityFrameworkCore;
using Proyecto_FInal_Grupo_1.Models;

namespace Proyecto_FInal_Grupo_1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Driver> Drivers => Set<Driver>();
        public DbSet<TeamCar> TeamCars => Set<TeamCar>();
        public DbSet<Sponsor> Sponsors => Set<Sponsor>();
        public DbSet<CarSponsor> CarSponsors => Set<CarSponsor>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación 1:1 (Driver <-> TeamCar)
            modelBuilder.Entity<Driver>()
                .HasOne(d => d.TeamCar)
                .WithOne(c => c.Driver)
                .HasForeignKey<Driver>(d => d.TeamCarId)
                .IsRequired(false); // Un auto puede no tener piloto asignado aun

            // Relación 1:N (Sponsor -> Drivers)
            modelBuilder.Entity<Driver>()
                .HasOne(d => d.Sponsor)
                .WithMany(s => s.Drivers)
                .HasForeignKey(d => d.SponsorId);

            // Relación N:M (CarSponsor)
            // No requiere configuración extra si las FKs están bien, pero aseguramos:
            modelBuilder.Entity<CarSponsor>()
                .HasOne(cs => cs.TeamCar)
                .WithMany(c => c.CarSponsors)
                .HasForeignKey(cs => cs.TeamCarId);

            modelBuilder.Entity<CarSponsor>()
                .HasOne(cs => cs.Sponsor)
                .WithMany(s => s.CarSponsors)
                .HasForeignKey(cs => cs.SponsorId);
        }
    }
}
