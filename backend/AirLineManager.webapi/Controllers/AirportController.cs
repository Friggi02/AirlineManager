using AirLineManager.dal.Entities;
using AirLineManager.webapi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirLineManager.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly Mapper _map;
        private readonly IUnitOfWork _repo;

        public AirportController(IUnitOfWork repo, Mapper mapper)
        {
            _repo = repo;
            _map = mapper;
        }

        [HttpGet]
        /*[Authorize]*/
        [Route("GetAll")]
        public IActionResult GetAll() => Ok(_repo.AirportRepo.GetAll().ConvertAll(_map.MapEntityToModel));

        [HttpGet]
        /*[Authorize]*/
        [Route("GetSearch")]
        public IActionResult GetSearch(string query)
        {
            query = query.Trim().ToLower();
            List<Airport> result = new();

            result.AddRange(_repo.AirportRepo.GetByFilter(a => a.Name.ToLower().StartsWith(query)));
            result.AddRange(_repo.AirportRepo.GetByFilter(a => a.City.ToLower().StartsWith(query)));
            result.AddRange(_repo.AirportRepo.GetByFilter(a => a.Iata.ToLower().StartsWith(query)));

            return Ok(result);
        }
    }
}