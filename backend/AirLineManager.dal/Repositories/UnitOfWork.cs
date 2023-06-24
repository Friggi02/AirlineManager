using AirlineManager.dal.Entities;
using AirLineManager.dal.Data;
using AirLineManager.dal.Entities;
using AirLineManager.dal.Repositories;

namespace AirLineManager.webapi
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AirlineDbContext _ctx;
        public IRepository<Aircraft> AircraftRepo { get; private set; }
        public IRepository<Airport> AirportRepo { get; private set; }
        public IRepository<AirRoute> AirRouteRepo { get; private set; }
        public IFlightRepository FlightRepo { get; private set; }
        public IRepository<Reservation> ReservationRepo { get; private set; }
        public IRepository<ReservedSeat> ReservedSeatRepo { get; private set; }
        public IRepository<Seat> SeatRepo { get; private set; }
        public IRepository<User> UserRepo { get; private set; }

        public UnitOfWork(AirlineDbContext ctx)
        {
            _ctx = ctx;
            AircraftRepo = new AircraftRepository(ctx);
            AirportRepo = new AirportRepository(ctx);
            AirRouteRepo = new AirRouteRepository(ctx);
            FlightRepo = new FlightRepository(ctx);
            ReservationRepo = new ReservationRepository(ctx);
            ReservedSeatRepo = new ReservedSeatRepository(ctx);
            SeatRepo = new SeatRepository(ctx);
            UserRepo = new UserRepository(ctx);
        }

        public bool Commit()
        {
            return _ctx.SaveChanges() > 0;
        }
    }
}
