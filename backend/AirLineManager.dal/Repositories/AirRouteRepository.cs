using AirlineManager.dal.Entities;
using AirLineManager.dal.Data;

namespace AirLineManager.dal.Repositories
{
    public class AirRouteRepository : GenericRepository<AirRoute>, IRepository<AirRoute>
    {
        public AirRouteRepository(AirlineDbContext ctx) : base(ctx)
        {
        }

        public ReturnRequest Update(AirRoute entity)
        {

            AirRoute? old = _ctx.AirRoutes.FirstOrDefault(airroute => airroute.AirRouteId == entity.AirRouteId);

            if (old != null)
            {
                old.AircraftId = entity.AircraftId;
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
