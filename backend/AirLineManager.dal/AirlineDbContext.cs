using AirlineManager.dal.Entities;
using AirLineManager.dal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AirLineManager.dal.Data
{
    public class AirlineDbContext : IdentityDbContext<IdentityUser>
    {
        public AirlineDbContext() : base() { }
        public AirlineDbContext(DbContextOptions<AirlineDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ReservedSeat>().HasKey(x => new { x.ReservationId, x.SeatId });

            builder.Entity<AirRoute>(entity =>
            {
                entity.HasOne(ar => ar.DepartureAirport)
                    .WithMany(ar => ar.DepartureAirRoutes)
                    .HasForeignKey(ar => ar.DepartureAirportId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ar => ar.ArrivalAirport)
                    .WithMany(ar => ar.ArrivalAirRoutes)
                    .HasForeignKey(ar => ar.ArrivalAirportId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => new { e.DepartureAirportId, e.ArrivalAirportId, e.AircraftId }).IsUnique();
            });

            base.OnModelCreating(builder);
        }

        public DbSet<Aircraft> Aircrafts { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<AirRoute> AirRoutes { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservedSeat> ReservedSeats { get; set; }
        public DbSet<Seat> Seats { get; set; }

    }
}