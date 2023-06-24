using AirlineManager.dal.Entities;
using AirLineManager.dal;
using AirLineManager.webapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirLineManager.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservedSeatController : ControllerBase
    {
        private readonly Mapper _map;
        private readonly IUnitOfWork _repo;
        private readonly Methods _methods;

        public ReservedSeatController(IUnitOfWork repo, Mapper mapper, Methods methods)
        {
            _repo = repo;
            _map = mapper;
            _methods = methods;
        }

        [HttpGet]
        /*[Authorize]*/
        [Route("GetAll")]
        public IActionResult GetAll() => Ok(_repo.ReservedSeatRepo.GetAll().ConvertAll(_map.MapEntityToModel));

        [HttpGet]
        /*[Authorize]*/
        [Route("GetSingle")]
        public IActionResult GetSingle(int id)
        {
            ReservedSeat? entity = _repo.ReservedSeatRepo.Get(id);
            if (entity == null) return Ok("Entity not found");
            return Ok(_map.MapEntityToModel(entity));
        }

        [HttpDelete]
        /*[Authorize]*/
        [Route("Delete")]
        public IActionResult Delete(ReservedSeatModel model)
        {
            ReturnRequest result = _methods.ReservedSeatSuspension(model.ReservationId, model.SeatId);
            if (result.Return) return Ok(result.MessageReturn);
            return NotFound(result.MessageReturn);
        }

        [HttpDelete]
        /*[Authorize]*/
        [Route("Restore")]
        public IActionResult Restore(ReservedSeatModel model)
        {
            ReturnRequest result = _methods.ReservedSeatRemoveSuspension(model.ReservationId, model.SeatId);
            if (result.Return) return Ok(result.MessageReturn);
            return NotFound(result.MessageReturn);
        }

        [HttpGet]
        /*[Authorize]*/
        [Route("GetByReservation")]
        public IActionResult GetByReservation(int id)
        {
            Reservation? entity = _repo.ReservationRepo.Get(entity => entity.ReservationId == id, "ReservedSeats");
            if (entity == null || entity.IsDeleted) return NotFound("Entity not found");

            List<ReservedSeatModel> models = new();

            foreach (ReservedSeat res in entity.ReservedSeats) models.Add(_map.MapEntityToModel(res));

            return Ok(models);
        }

    }
}
