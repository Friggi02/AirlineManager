using AirlineManager.dal.Entities;

namespace AirLineManager.dal.Repositories
{
    public interface IFlightRepository : IRepository<Flight>
    {
        public void Copy(DateTime day, DateTime startperiod, DateTime endperiod);
    }
}
