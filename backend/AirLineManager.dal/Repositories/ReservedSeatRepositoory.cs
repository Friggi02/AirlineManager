using AirlineManager.dal.Entities;
using AirLineManager.dal.Data;

namespace AirLineManager.dal.Repositories
{
    public class ReservedSeatRepository : GenericRepository<ReservedSeat>, IRepository<ReservedSeat>
    {
        public ReservedSeatRepository(AirlineDbContext ctx) : base(ctx)
        {
        }
        public ReturnRequest Update(ReservedSeat entity)
        {

            ReservedSeat? old = _ctx.ReservedSeats.FirstOrDefault(reservedSeat => reservedSeat.ReservationId == entity.ReservationId && reservedSeat.SeatId == entity.SeatId);

            if (old != null)
            {
                old.SeatCode = entity.SeatCode;
                old.PassengerName = entity.PassengerName;
                old.PassengerSurname = entity.PassengerSurname;
                old.PassengerAge = entity.PassengerAge;
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
