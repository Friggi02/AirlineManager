using AirlineManager.dal.Entities;
using AirLineManager.dal.Entities;
using AirLineManager.dal.Repositories;

namespace AirLineManager.webapi
{
    public interface IUnitOfWork
    {
        IRepository<Aircraft> AircraftRepo { get; }
        IRepository<Airport> AirportRepo { get; }
        IRepository<AirRoute> AirRouteRepo { get; }
        IFlightRepository FlightRepo { get; }
        IRepository<Reservation> ReservationRepo { get; }
        IRepository<ReservedSeat> ReservedSeatRepo { get; }
        IRepository<Seat> SeatRepo { get; }
        IRepository<User> UserRepo { get; }

        bool Commit();
    }
}