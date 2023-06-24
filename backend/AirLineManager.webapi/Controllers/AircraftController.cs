using AirlineManager.dal.Entities;
using AirLineManager.webapi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirLineManager.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftController : ControllerBase
    {
        private readonly Mapper _map;
        private readonly IUnitOfWork _repo;

        public AircraftController(IUnitOfWork repo, Mapper mapper)
        {
            _repo = repo;
            _map = mapper;
        }

        [HttpGet]
        /*[Authorize]*/
        [Route("GetAll")]
        public IActionResult GetAll() => Ok(_repo.AircraftRepo.GetAll().ConvertAll(_map.MapEntityToModel));

        [HttpGet]
        /*[Authorize]*/
        [Route("GetSingle")]
        public IActionResult GetSingle(int id)
        {
            Aircraft? entity = _repo.AircraftRepo.Get(id);
            if (entity == null) return NotFound("Entity not found");
            return Ok(_map.MapEntityToModel(entity));
        }

        [HttpGet]
        /*[Authorize]*/
        [Route("GetSingleWithSeats")]
        public IActionResult GetWithSeats(int id)
        {
            Aircraft? entity = _repo.AircraftRepo.Get(entity => entity.AircraftId == id, "Seats");
            if (entity == null) return NotFound("Entity not found");
            return Ok(_map.MapEntityToModelComplete(entity));
        }
    }
}