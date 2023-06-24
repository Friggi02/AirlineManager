using AirlineManager.dal.Entities;
using AirLineManager.dal.Entities;
using AirLineManager.webapi;

namespace AirLineManager.dal
{
    public class Methods
    {
        private readonly IUnitOfWork _repo;

        public Methods(IUnitOfWork repo)
        {
            _repo = repo;
        }

        public List<Seat> GetFreeSeats(int FlightId)
        {
            //get the flight and check if exist
            Flight? flight = _repo.FlightRepo.Get(FlightId);
            if (flight == null || flight.IsDeleted) return new List<Seat>();

            //get all the aircraft seat
            AirRoute? airroute = _repo.AirRouteRepo.Get(flight.AirRouteId);
            Aircraft? aircraft = _repo.AircraftRepo.Get(a => a.AircraftId == airroute.AircraftId, "Seats");

            //get all the reservation of the flight
            List<Reservation> reservations = _repo.ReservationRepo.GetByFilter(r => r.FlightId == FlightId);

            //get all the reserved seat of the flight
            List<ReservedSeat> reservedseats = new();
            foreach (Reservation res in reservations)
            {
                Reservation? reservation = _repo.ReservationRepo.Get(r => r.ReservationId == res.ReservationId, "ReservedSeats");
                reservedseats.AddRange(reservation.ReservedSeats);
            }

            return aircraft.Seats.Where(s => reservedseats.SingleOrDefault(a => a.SeatCode == s.Code) == null).ToList();
        }

        public AirRoute GetRouteFromAirports(int departure_id, int arrival_id)
        {
            List<AirRoute> airroutes = _repo.AirRouteRepo.GetByFilter(r => r.DepartureAirportId == departure_id && r.ArrivalAirportId == arrival_id);

            if (airroutes == null || airroutes.Count == 0) return null;

            return airroutes[0];
        }

        public AirRoute GetRouteFromAirports(string departure, string arrival)
        {
            departure = departure.Trim().ToLower();
            arrival = arrival.Trim().ToLower();

            List<Airport>? departure_airport = _repo.AirportRepo.GetByFilter(r => r.Name.ToLower() == departure);
            List<Airport>? arrival_airport = _repo.AirportRepo.GetByFilter(r => r.Name.ToLower() == arrival);

            if (departure_airport == null || departure_airport.Count == 0) return null;
            if (arrival_airport == null || arrival_airport.Count == 0) return null;

            List<AirRoute>? airroutes = _repo.AirRouteRepo.GetByFilter(r => r.DepartureAirportId == departure_airport[0].AirportId && r.ArrivalAirportId == arrival_airport[0].AirportId);

            if (airroutes == null || airroutes.Count == 0) return null;

            return airroutes[0];
        }

        public ReturnRequest AirRouteSuspension(int id)
        {
            AirRoute? airroute = _repo.AirRouteRepo.Get(r => r.AirRouteId == id, "Flights");
            if (airroute == null || airroute.IsDeleted) return new ReturnRequest()
            {
                Return = false,
                MessageReturn = "Entity not found"
            };

            airroute.IsDeleted = true;
            foreach (Flight flight in _repo.FlightRepo.GetByFilter(f => f.AirRouteId == id)) FlightSuspension(flight.FlightId);

            return _repo.AirRouteRepo.Update(airroute);
        }

        public ReturnRequest FlightSuspension(int id)
        {
            Flight? flight = _repo.FlightRepo.Get(r => r.FlightId == id, "Reservations");
            if (flight == null || flight.IsDeleted) return new ReturnRequest()
            {
                Return = false,
                MessageReturn = "Entity not found"
            };

            flight.IsDeleted = true;

            foreach (Reservation reservation in flight.Reservations) ReservationSuspension(reservation.ReservationId);

            return _repo.FlightRepo.Update(flight);
        }

        public ReturnRequest ReservationSuspension(int id)
        {
            Reservation? reservation = _repo.ReservationRepo.Get(r => r.ReservationId == id, "ReservedSeats");
            if (reservation == null || reservation.IsDeleted) return new ReturnRequest()
            {
                Return = false,
                MessageReturn = "Entity not found"
            };

            reservation.IsDeleted = true;

            foreach (ReservedSeat reservedseat in reservation.ReservedSeats) ReservedSeatSuspension(reservation.ReservationId, reservedseat.SeatId);

            return _repo.ReservationRepo.Update(reservation);
        }

        public ReturnRequest ReservedSeatSuspension(int ReservationId, int SeatId)
        {
            ReservedSeat? reservedseat = _repo.ReservedSeatRepo.GetByFilter(r => r.ReservationId == ReservationId && r.SeatId == SeatId).First();
            if (reservedseat == null || reservedseat.IsDeleted) return new ReturnRequest()
            {
                Return = false,
                MessageReturn = "Entity not found"
            };
            reservedseat.IsDeleted = true;
            return _repo.ReservedSeatRepo.Update(reservedseat);
        }

        public ReturnRequest AirRouteRemoveSuspension(int id)
        {
            AirRoute? airroute = _repo.AirRouteRepo.Get(r => r.AirRouteId == id, "Flights");
            if (airroute == null || !airroute.IsDeleted) return new ReturnRequest()
            {
                Return = false,
                MessageReturn = "Entity not found"
            };

            airroute.IsDeleted = false;
            foreach (Flight flight in _repo.FlightRepo.GetByFilter(f => f.AirRouteId == id)) FlightRemoveSuspension(flight.FlightId);

            return _repo.AirRouteRepo.Update(airroute);
        }

        public ReturnRequest FlightRemoveSuspension(int id)
        {
            Flight? flight = _repo.FlightRepo.Get(r => r.FlightId == id, "Reservations");
            if (flight == null || !flight.IsDeleted) return new ReturnRequest()
            {
                Return = false,
                MessageReturn = "Entity not found"
            };

            flight.IsDeleted = false;

            foreach (Reservation reservation in flight.Reservations) ReservationRemoveSuspension(reservation.ReservationId);

            return _repo.FlightRepo.Update(flight);
        }

        public ReturnRequest ReservationRemoveSuspension(int id)
        {
            Reservation? reservation = _repo.ReservationRepo.Get(r => r.ReservationId == id, "ReservedSeats");
            if (reservation == null || !reservation.IsDeleted) return new ReturnRequest()
            {
                Return = false,
                MessageReturn = "Entity not found"
            };

            reservation.IsDeleted = false;

            foreach (ReservedSeat reservedseat in reservation.ReservedSeats) ReservedSeatRemoveSuspension(reservation.ReservationId, reservedseat.SeatId);

            return _repo.ReservationRepo.Update(reservation);
        }

        public ReturnRequest ReservedSeatRemoveSuspension(int ReservationId, int SeatId)
        {
            ReservedSeat? reservedseat = _repo.ReservedSeatRepo.GetByFilter(r => r.ReservationId == ReservationId && r.SeatId == SeatId).First();
            if (reservedseat == null || !reservedseat.IsDeleted) return new ReturnRequest()
            {
                Return = false,
                MessageReturn = "Entity not found"
            };
            reservedseat.IsDeleted = false;
            return _repo.ReservedSeatRepo.Update(reservedseat);
        }
    }
}