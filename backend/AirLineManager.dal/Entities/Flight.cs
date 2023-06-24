using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AirlineManager.dal.Entities
{
    public class Flight
    {
        public int FlightId { get; set; }
        [Required, NotNull] public int AirRouteId { get; set; }
        [Required, NotNull] public DateTime DepartureTime { get; set; }
        [Required, NotNull] public DateTime ArrivalTime { get; set; }
        public bool IsDeleted { get; set; }
        public AirRoute AirRoute { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}