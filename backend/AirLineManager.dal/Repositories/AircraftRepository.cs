using AirlineManager.dal.Entities;
using AirLineManager.dal.Data;

namespace AirLineManager.dal.Repositories
{
    public class AircraftRepository : GenericRepository<Aircraft>, IRepository<Aircraft>
    {
        public AircraftRepository(AirlineDbContext ctx) : base(ctx)
        {
        }

        public ReturnRequest Update(Aircraft entity)
        {
            throw new NotImplementedException();
        }
    }
}
