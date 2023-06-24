using AirlineManager.dal.Entities;
using AirLineManager.dal.Data;

namespace AirLineManager.dal.Repositories
{
    public class ReservationRepository : GenericRepository<Reservation>, IRepository<Reservation>
    {
        public ReservationRepository(AirlineDbContext ctx) : base(ctx)
        {
        }

        public ReturnRequest Update(Reservation entity)
        {

            Reservation? old = _ctx.Reservations.FirstOrDefault(reservation => reservation.ReservationId == entity.ReservationId);

            if (old != null)
            {
                old.FlightId = entity.FlightId;
                old.IsDeleted = entity.IsDeleted;
                _ctx.SaveChanges();
                return new ReturnRequest()
                {
                    Return = true,
                    MessageReturn = "Entity updated successfully"
                };
            }
            else
            {
                return new ReturnRequest()
                {
                    Return = false,
                    MessageReturn = "Entity not found"
                };
            }
        }
    }
}
