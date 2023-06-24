using AirlineManager.dal.Entities;
using AirLineManager.dal;
using AirLineManager.webapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirLineManager.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirRouteController : ControllerBase
    {
        private readonly Mapper _map;
        private readonly IUnitOfWork _repo;
        private readonly Methods _methods;

        public AirRouteController(IUnitOfWork repo, Mapper mapper, Methods methods)
        {
            _repo = repo;
            _map = mapper;
            _methods = methods;
        }

        [HttpGet]
        /*[Authorize]*/
        [Route("GetAll")]
        public IActionResult GetAll() => Ok(_repo.AirRouteRepo.GetAll().ConvertAll(_map.MapEntityToModel));

        [HttpGet]
        /*[Authorize]*/
        [Route("GetSingle")]
        public IActionResult GetSingle(int id)
        {
            AirRoute? entity = _repo.AirRouteRepo.Get(id);
            if (entity == null) return NotFound("Entity not found");
            return Ok(_map.MapEntityToModel(entity));
        }

        [HttpGet]
        /*[Authorize]*/
        [Route("GetSingleWithFlights")]
        public IActionResult GetSingleWithFlights(int id)
        {
            AirRoute? entity = _repo.AirRouteRepo.Get(entity => entity.AirRouteId == id, "Flights");
            if (entity == null) return NotFound("Entity not found");
            return Ok(_map.MapEntityToModel(entity));
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("Create")]
        public IActionResult Create(AirRouteModel model)
        {
            if (_repo.AirportRepo.Get(model.DepartureAirportId) == null) return BadRequest("DepartureAirportId not valid");
            if (_repo.AirportRepo.Get(model.ArrivalAirportId) == null) return BadRequest("ArrivalAirportId not valid");
            if (model.DepartureAirportId == model.ArrivalAirportId) return BadRequest("DepartureAirportId and ArrivalAirportId cannot be the same");
            if (_repo.AircraftRepo.Get(model.AircraftId) == null) return BadRequest("AircraftId not valid");

            model.IsDeleted = false;

            _repo.AirRouteRepo.Create(_map.MapModelToEntity(model));

            return Ok("Entity created successfully");
        }

        [HttpDelete]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            ReturnRequest result = _methods.AirRouteSuspension(id);
            if (result.Return) return Ok(result.MessageReturn);
            return NotFound(result.MessageReturn);
        }

        [HttpDelete]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("Restore")]
        public IActionResult Restore(int id)
        {
            ReturnRequest result = _methods.AirRouteRemoveSuspension(id);
            if (result.Return) return Ok(result.MessageReturn);
            return NotFound(result.MessageReturn);
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("Update")]
        public IActionResult Update(AirRouteModel model)
        {
            AirRoute? old = _repo.AirRouteRepo.Get(model.AirRouteId);

            if (old == null || old.IsDeleted) return NotFound("Entity not found");
            if (_repo.AircraftRepo.Get(model.AircraftId) == null) return BadRequest("AircraftId not valid");

            ReturnRequest result = _repo.AirRouteRepo.Update(_map.MapModelToEntity(model));
            if (result.Return) return Ok(result.MessageReturn);
            return NotFound(result.MessageReturn);
        }
    }
}