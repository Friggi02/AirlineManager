using System.ComponentModel.DataAnnotations.Schema;
using AirLineManager.dal.Entities;

namespace AirlineManager.dal.Entities
{
    public class AirRoute
    {
        public int AirRouteId { get; set; }
        public int DepartureAirportId { get; set; }
        public int ArrivalAirportId { get; set; }
        public int AircraftId { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("DepartureAirportId")] public Airport DepartureAirport { get; set; } = new Airport();
        [ForeignKey("ArrivalAirportId")] public Airport ArrivalAirport { get; set; } = new Airport();
        public Aircraft Aircraft { get; set; } = new Aircraft();
    }
}