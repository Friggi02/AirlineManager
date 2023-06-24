using AirLineManager.dal.Data;
using AirLineManager.dal.Entities;

namespace AirLineManager.dal.Repositories
{
    public class AirportRepository : GenericRepository<Airport>, IRepository<Airport>
    {
        public AirportRepository(AirlineDbContext ctx) : base(ctx)
        {
        }
        public ReturnRequest Update(Airport entity)
        {
            throw new NotImplementedException();
        }
    }
}
