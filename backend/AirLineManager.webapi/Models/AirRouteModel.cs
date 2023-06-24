using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AirlineManager.dal.Entities
{
    public class AirRouteModel
    {
        public int AirRouteId { get; set; }
        [Required, NotNull] public int DepartureAirportId { get; set; }
        [Required, NotNull] public int ArrivalAirportId { get; set; }
        public string? DepartureAirportName { get; set; }
        public string? ArrivalAirportName { get; set; }
        [Required, NotNull] public int AircraftId { get; set; }
        public bool IsDeleted { get; set; }
        public List<FlightModel>? Flights { get; set; }
    }
}