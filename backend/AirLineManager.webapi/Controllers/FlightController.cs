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
    public class FlightController : ControllerBase
    {
        private readonly Mapper _map;
        private readonly IUnitOfWork _repo;
        private readonly Methods _methods;

        public FlightController(IUnitOfWork repo, Mapper mapper, Methods methods)
        {
            _repo = repo;
            _map = mapper;
            _methods = methods;
        }

        [HttpGet]
        /*[Authorize]*/
        [Route("GetAll")]
        public IActionResult GetAll() => Ok(_repo.FlightRepo.GetAll().ConvertAll(MapEntityToModelComplete));

        [HttpGet]
        /*[Authorize]*/
        [Route("GetSingle")]
        public IActionResult GetSingle(int id)
        {
            Flight? entity = _repo.FlightRepo.Get(id);
            if (entity == null) return NotFound("Entity not found");
            return Ok(MapEntityToModelComplete(entity));
        }

        [HttpPost]
        /*[Authorize]*/
        [Route("SearchingFlights")]
        public IActionResult SearchingFlights(JourneyModel input)
        {
            //check if the airroute is valid
            AirRoute? airroute = _methods.GetRouteFromAirports(input.DepartureAirport, input.ArrivalAirport);
            if (airroute == null || airroute.IsDeleted) return BadRequest("Route not found");

            //check the departure date
            if (input.StartPeriod < DateTime.Today) return BadRequest("StartPeriod not valid");

            //check the arrival date
            if (input.EndPeriod == null)
                input.EndPeriod = input.StartPeriod.AddDays(7);
            else
                if (input.StartPeriod > input.EndPeriod) return BadRequest("EndPeriod not valid");

            List<Flight>? flights = _repo.FlightRepo.GetByFilter(f => f.AirRouteId == airroute.AirRouteId && f.DepartureTime >= input.StartPeriod && f.DepartureTime <= input.EndPeriod);

            return Ok(flights.ConvertAll(MapEntityToModelComplete));
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("Create")]
        public IActionResult Create(FlightModel model)
        {
            AirRoute? route = _repo.AirRouteRepo.Get(model.AirRouteId);
            if (route == null || route.IsDeleted) return BadRequest("AirRouteId not valid");
            if (model.DepartureTime < DateTime.Now) return BadRequest("DepartureTime not valid");
            if (model.ArrivalTime < model.DepartureTime) return BadRequest("ArrivalTime not valid");

            model.IsDeleted = false;

            _repo.FlightRepo.Create(_map.MapModelToEntity(model));

            return Ok("Entity created successfully");
        }

        [HttpDelete]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            Flight? old = _repo.FlightRepo.Get(id);
            if (old == null || old.IsDeleted) return NotFound("Entity not found");
            if (old.DepartureTime <= DateTime.Now && old.Reservations != null) return BadRequest("Non-cancellable flight");

            ReturnRequest result = _repo.FlightRepo.Delete(id);
            if (result.Return) return Ok(result.MessageReturn);
            return NotFound(result.MessageReturn);
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("Update")]
        public IActionResult Update(FlightModel model)
        {
            Flight? old = _repo.FlightRepo.Get(model.FlightId);
            if (old == null || old.IsDeleted) return NotFound("Entity not found");

            AirRoute? route = _repo.AirRouteRepo.Get(model.AirRouteId);
            if (route == null || route.IsDeleted) return BadRequest("AirRouteId not valid");
            if (model.DepartureTime < DateTime.Now) return BadRequest("DepartureTime not valid");
            if (model.ArrivalTime < model.DepartureTime) return BadRequest("ArrivalTime not valid");
            if (old.AirRouteId != model.AirRouteId && old.Reservations != null) return BadRequest("Non-updatable flight");

            ReturnRequest result = _repo.FlightRepo.Update(_map.MapModelToEntity(model));
            if (result.Return) return Ok(result.MessageReturn);
            return NotFound(result.MessageReturn);
        }

        [HttpGet]
        /*[Authorize]*/
        [Route("GetFlightsByRoute")]
        public IActionResult GetFlightsByRoute(int id)
        {
            return Ok(_repo.FlightRepo.GetByFilter(f => f.AirRouteId == id).ConvertAll(MapEntityToModelComplete));
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("Duplicate")]
        public IActionResult Duplicate(DuplicateFlightsModel input)
        {
            if (input.StartPeriod > input.EndPeriod) return BadRequest("Period not valid");
            if (input.StartPeriod.Date < DateTime.Now.Date) return BadRequest("Period not valid");

            _repo.FlightRepo.Copy(input.Day, input.StartPeriod, input.EndPeriod);
            return Ok();
        }

        private FlightModel MapEntityToModelComplete(Flight entity)
        {
            FlightModel model = new();
            AirRoute? airroute = _repo.AirRouteRepo.Get(entity.AirRouteId);
            Aircraft? aircraft = _repo.AircraftRepo.Get(x => airroute.AircraftId == x.AircraftId, "Seats");
            Airport? departureairport = _repo.AirportRepo.Get(airroute.DepartureAirportId);
            Airport? arrivalairport = _repo.AirportRepo.Get(airroute.ArrivalAirportId);

            model.FlightId = entity.FlightId;
            model.AirRouteId = entity.AirRouteId;
            model.DepartureTime = entity.DepartureTime;
            model.ArrivalTime = entity.ArrivalTime;
            model.IsDeleted = entity.IsDeleted;
            model.DepartureAirport = departureairport.Name;
            model.ArrivalAirport = arrivalairport.Name;
            model.AircraftId = airroute.AircraftId;
            List<Seat> seats = _methods.GetFreeSeats(entity.FlightId);
            if (seats != null)
                model.FreeSeats = seats.Count;
            else
                model.FreeSeats = 0;
            model.TotalSeats = aircraft.Seats.Count;
            model.ReservedSeats = model.TotalSeats - model.FreeSeats;
            //if (departureairport.IsSchengen && arrivalairport.IsSchengen)
            //    model.IsSchengen = true;
            //else
            //    model.IsSchengen = false;
            return model;
        }

    }
}