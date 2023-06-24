using AirlineManager.dal.Entities;
using AirLineManager.dal.Entities;

namespace AirLineManager.webapi.Models
{
    public class Mapper
    {

        #region Aircraft
        public AircraftModel MapEntityToModel(Aircraft entity)
        {
            return new AircraftModel
            {
                AircraftId = entity.AircraftId,
                Name = entity.Name
            };
        }
        public AircraftModel MapEntityToModelComplete(Aircraft entity)
        {
            AircraftModel model = new AircraftModel
            {
                AircraftId = entity.AircraftId,
                Name = entity.Name
            };
            if (entity.Seats != null) model.Seats = entity.Seats.ConvertAll(MapEntityToModel);

            return model;
        }
        public Aircraft MapModelToEntity(AircraftModel model)
        {
            return new Aircraft
            {
                AircraftId = model.AircraftId,
                Name = model.Name
            };
        }
        #endregion

        #region Airport
        public AirportModel MapEntityToModel(Airport entity)
        {
            return new AirportModel
            {
                AirportId = entity.AirportId,
                Icao = entity.Icao,
                Iata = entity.Iata,
                Name = entity.Name,
                City = entity.City,
                State = entity.State,
                Country = entity.Country,
                Elevation = entity.Elevation,
                Lat = entity.Lat,
                Lon = entity.Lon,
                Tz = entity.Tz,
            };
        }

        public Airport MapModelToEntity(AirportModel model)
        {
            return new Airport
            {
                AirportId = model.AirportId,
                Icao = model.Icao,
                Iata = model.Iata,
                Name = model.Name,
                City = model.City,
                State = model.State,
                Country = model.Country,
                Elevation = model.Elevation,
                Lat = model.Lat,
                Lon = model.Lon,
                Tz = model.Tz,
            };
        }
        #endregion

        #region AirRoute
        public AirRouteModel MapEntityToModel(AirRoute entity)
        {
            return new AirRouteModel
            {
                AirRouteId = entity.AirRouteId,
                DepartureAirportId = entity.DepartureAirportId,
                ArrivalAirportId = entity.ArrivalAirportId,
                AircraftId = entity.AircraftId,
                IsDeleted = entity.IsDeleted,
            };
        }

        public AirRoute MapModelToEntity(AirRouteModel model)
        {
            return new AirRoute
            {
                AirRouteId = model.AirRouteId,
                DepartureAirportId = model.DepartureAirportId,
                ArrivalAirportId = model.ArrivalAirportId,
                AircraftId = model.AircraftId,
                IsDeleted = model.IsDeleted
            };
        }
        #endregion

        #region Flight
        public FlightModel MapEntityToModel(Flight entity)
        {
            return new FlightModel
            {
                FlightId = entity.FlightId,
                AirRouteId = entity.AirRouteId,
                DepartureTime = entity.DepartureTime,
                ArrivalTime = entity.ArrivalTime,
                IsDeleted = entity.IsDeleted
            };
        }

        public Flight MapModelToEntity(FlightModel model)
        {
            return new Flight
            {
                FlightId = model.FlightId,
                AirRouteId = model.AirRouteId,
                DepartureTime = model.DepartureTime,
                ArrivalTime = model.ArrivalTime,
                IsDeleted = model.IsDeleted
            };
        }
        #endregion

        #region Reservation
        public ReservationModel MapEntityToModel(Reservation entity)
        {
            return new ReservationModel
            {
                ReservationId = entity.ReservationId,
                FlightId = entity.FlightId,
                IsDeleted = entity.IsDeleted
            };
        }
        public ReservationModel MapEntityToModelComplete(Reservation entity)
        {
            return new ReservationModel
            {
                ReservationId = entity.ReservationId,
                FlightId = entity.FlightId,
                IsDeleted = entity.IsDeleted,
                ReservedSeats = entity.ReservedSeats.ConvertAll(MapEntityToModel)
            };
        }
        public Reservation MapModelToEntity(ReservationModel model)
        {
            return new Reservation
            {
                ReservationId = model.ReservationId,
                FlightId = model.FlightId,
                UserId = model.UserId,
                IsDeleted = model.IsDeleted
            };
        }
        #endregion

        #region ReservedSeat
        public ReservedSeatModel MapEntityToModel(ReservedSeat entity)
        {
            return new ReservedSeatModel
            {
                SeatCode = entity.SeatCode,
                PassengerName = entity.PassengerName,
                PassengerSurname = entity.PassengerSurname,
                PassengerAge = entity.PassengerAge,
                ReservationId = entity.ReservationId,
                SeatId = entity.SeatId,
                IsDeleted = entity.IsDeleted
            };
        }

        public ReservedSeat MapModelToEntity(ReservedSeatModel model)
        {
            return new ReservedSeat
            {
                SeatCode = model.SeatCode,
                PassengerName = model.PassengerName,
                PassengerSurname = model.PassengerSurname,
                PassengerAge = model.PassengerAge,
                ReservationId = model.ReservationId,
                SeatId = model.SeatId,
                IsDeleted = model.IsDeleted
            };
        }
        #endregion

        #region Seat
        public SeatModel MapEntityToModel(Seat entity)
        {
            return new SeatModel
            {
                SeatId = entity.SeatId,
                AircraftId = entity.AircraftId,
                Code = entity.Code,
                Row = entity.Row,
                Column = entity.Column,
                Category = entity.Category,
            };
        }

        public Seat MapModelToEntity(SeatModel model)
        {
            return new Seat
            {
                SeatId = model.SeatId,
                AircraftId = model.AircraftId,
                Code = model.Code,
                Row = model.Row,
                Column = model.Column,
                Category = model.Category,
            };
        }
        #endregion

        #region User
        public UserModel MapEntityToModel(User entity, string role)
        {
            return new UserModel
            {
                UserId = entity.Id,
                UserName = entity.UserName,
                Email = entity.Email,
                IsDeleted = entity.IsDeleted,
                Role = role
            };
        }
        public UserModel MapEntityToModelComplete(User entity)
        {
            UserModel model = new()
            {
                UserId = entity.Id,
                UserName = entity.UserName,
                Email = entity.Email,
                IsDeleted = entity.IsDeleted
            };
            if (entity.Reservations != null) model.Reservations = entity.Reservations.ConvertAll(MapEntityToModel);

            return model;
        }
        public User MapModelToEntity(UserModel model)
        {
            return new User()
            {
                Id = model.UserId,
                UserName = model.UserName,
                Email = model.Email,
                IsDeleted = model.IsDeleted
            };
        }
        #endregion
    }
}