using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AirlineManager.dal.Entities
{
    public class FlightModel
    {
        public int FlightId { get; set; }
        [Required, NotNull] public int AirRouteId { get; set; }
        [Required, NotNull] public DateTime DepartureTime { get; set; }
        [Required, NotNull] public DateTime ArrivalTime { get; set; }
        public bool IsDeleted { get; set; }
        public string? DepartureAirport { get; set; }
        public string? ArrivalAirport { get; set; }
        public int FreeSeats { get; set; }
        public int ReservedSeats { get; set; }
        public int TotalSeats { get; set; }
        public bool IsSchengen { get; set; }
        public int? AircraftId { get; set; }
    }
}