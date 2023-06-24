using System.Security.Claims;
using AirlineManager.dal.Entities;
using AirLineManager.dal;
using AirLineManager.dal.Entities;
using AirLineManager.webapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirLineManager.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly Mapper _map;
        private readonly IUnitOfWork _repo;
        private readonly Methods _methods;

        public ReservationController(IUnitOfWork repo, Mapper mapper, Methods methods)
        {
            _repo = repo;
            _map = mapper;
            _methods = methods;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("GetAll")]
        public IActionResult GetAll() => Ok(_repo.ReservationRepo.GetAll().ConvertAll(_map.MapEntityToModel));

        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("GetSingle")]
        public IActionResult GetSingle(int id)
        {
            Reservation? entity = _repo.ReservationRepo.Get(id);
            if (entity == null) return Ok("Entity not found");
            return Ok(_map.MapEntityToModel(entity));
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("GetSingleWithReservedSeats")]
        public IActionResult GetSingleWithReservedSeats(int id)
        {
            Reservation? entity = _repo.ReservationRepo.Get(entity => entity.ReservationId == id, "ReservedSeats");
            if (entity == null) return Ok("Entity not found");
            return Ok(_map.MapEntityToModelComplete(entity));
        }

        [HttpPost]
        /*[Authorize]*/
        [Route("Create")]
        public IActionResult Create(ReservationModel model)
        {
            //get the flight and check if exist
            Flight? flight = _repo.FlightRepo.Get(model.FlightId);
            if (flight == null || flight.IsDeleted) return BadRequest("FlightId not valid");

            //get the user and check if exist
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userName)) return NotFound("UserId not found");
            List<User>? entity = _repo.UserRepo.GetByFilter(u => u.UserName == userName);
            if (entity == null || entity.Count == 0 || entity[0].IsDeleted) return NotFound("User not found");


            //check all the seats requested
            if (model.ReservedSeats == null) return BadRequest("ReservedSeats not valid");

            List<Seat> free_seats = _methods.GetFreeSeats(model.FlightId);

            foreach (ReservedSeatModel seat in model.ReservedSeats)
            {
                if (string.IsNullOrEmpty(seat.PassengerName)) return BadRequest("PassengerName not valid");
                if (string.IsNullOrEmpty(seat.PassengerSurname)) return BadRequest("PassengerName not valid");
                if (seat.PassengerAge < 0) return BadRequest("PassengerAge not valid");
                if (!free_seats.Any(s => s.Code == seat.SeatCode)) return BadRequest("SeatCode not valid or already booked");
                seat.IsDeleted = false;
            }

            model.IsDeleted = false;

            Reservation res = _map.MapModelToEntity(model);
            res.UserId = entity[0].Id;

            _repo.ReservationRepo.Create(res);

            int index = _repo.ReservationRepo.GetAll().Max(s => s.ReservationId);

            foreach (ReservedSeatModel seat in model.ReservedSeats)
            {
                seat.ReservationId = index;
                _repo.ReservedSeatRepo.Create(_map.MapModelToEntity(seat));
            }

            return Ok("Entity created successfully");
        }


        [HttpDelete]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            ReturnRequest result = _methods.ReservationSuspension(id);
            if (result.Return) return Ok(result.MessageReturn);
            return NotFound(result.MessageReturn);
        }

        [HttpDelete]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("Restore")]
        public IActionResult Restore(int id)
        {
            ReturnRequest result = _methods.ReservationRemoveSuspension(id);
            if (result.Return) return Ok(result.MessageReturn);
            return NotFound(result.MessageReturn);
        }

        [HttpGet]
        /*[Authorize]*/
        [Route("GetByUser")]
        public IActionResult GetByUser()
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userName)) return NotFound("UserId not found");

            User? entity = _repo.UserRepo.Get(u => u.UserName == userName, "Reservations");
            if (entity == null) return NotFound("User not found");

            return Ok(_map.MapEntityToModelComplete(entity));
        }

        /*
        [HttpPut]
        public IActionResult Update(ReservationModel model)
        {
            //get the old reservation and check if exist
            Reservation? old = _repo.ReservationRepo.Get(model.ReservationId);
            if (old == null || old.IsDeleted) return BadRequest("Entity not found");

            //get the flight and check if exist
            Flight? flight = _repo.FlightRepo.Get(model.FlightId);
            if (flight == null || flight.IsDeleted) return BadRequest("FlightId not valid");

            //check if there is at least one place reserved
            if (model.ReservedSeats == null) return BadRequest("ReservedSeats not valid");

            //get all the reservation of the flight
            List<Reservation> reservations = _repo.ReservationRepo.GetByFilter(r => r.FlightId == model.FlightId);

            //get all the reserved seat of the flight
            List<ReservedSeat> reservedseats_tot = new();
            foreach (Reservation reservation in reservations)
            {
                Reservation? res = _repo.ReservationRepo.Get(r => r.ReservationId == model.ReservationId, "ReservedSeats");
                reservedseats_tot.AddRange(reservation.ReservedSeats);
            }

            //get all the aircraft seat
            AirRoute? airroute = _repo.AirRouteRepo.Get(flight.AirRouteId);
            Aircraft? aircraft = _repo.AircraftRepo.Get(a => a.AircraftId == airroute.AircraftId, "Seats");

            foreach (ReservedSeatModel seat in model.ReservedSeats)
            {
                if (aircraft.Seats.SingleOrDefault(s => s.Code == seat.SeatCode) == null) return BadRequest("SeatCode not valid");
                if (reservedseats_tot.SingleOrDefault(r => r.SeatCode == seat.SeatCode) != null) return BadRequest("Seat already booked");
                seat.IsDeleted = false;
                seat.ReservationId = model.ReservationId;
            }

            foreach (ReservedSeatModel seat in model.ReservedSeats)
            {
                seat.ReservationId = model.ReservationId;
                _repo.ReservedSeatRepo.Update(_map.MapModelToEntity(seat));
            }

            ReturnRequest result = _repo.ReservationRepo.Update(_map.MapModelToEntity(model));
            if (result.Return) return Ok(result.MessageReturn);
            return BadRequest(result.MessageReturn);
        }
        */
    }
}
