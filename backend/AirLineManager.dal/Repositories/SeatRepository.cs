using AirLineManager.dal.Data;
using AirLineManager.dal.Entities;

namespace AirLineManager.dal.Repositories
{
    public class SeatRepository : GenericRepository<Seat>, IRepository<Seat>
    {
        public SeatRepository(AirlineDbContext ctx) : base(ctx)
        {
        }
        public ReturnRequest Update(Seat entity)
        {
            throw new NotImplementedException();
        }
    }
}
