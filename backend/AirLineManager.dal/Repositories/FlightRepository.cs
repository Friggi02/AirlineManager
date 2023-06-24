using AirlineManager.dal.Entities;
using AirLineManager.dal.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AirLineManager.dal.Repositories
{
    public class FlightRepository : GenericRepository<Flight>, IFlightRepository
    {
        public FlightRepository(AirlineDbContext ctx) : base(ctx)
        {
        }

        public ReturnRequest Update(Flight entity)
        {

            Flight? old = Get(entity.FlightId);

            if (old != null)
            {
                old.AirRouteId = entity.AirRouteId;
                old.DepartureTime = entity.DepartureTime;
                old.ArrivalTime = entity.ArrivalTime;
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

        public void Copy(DateTime day, DateTime startperiod, DateTime endperiod) => _ctx.Database.ExecuteSqlRaw("exec DuplicateFlights @day, @startperiod, @endperiod",
                                                                                                                new SqlParameter("@day", day.Date),
                                                                                                                new SqlParameter("@startperiod", startperiod.Date),
                                                                                                                new SqlParameter("@endperiod", endperiod.Date));


    }
}
