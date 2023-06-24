using System.Diagnostics.CodeAnalysis;
using AirlineManager.dal.Entities;

namespace AirLineManager.dal.Entities
{
    public class Airport
    {
        public int AirportId { get; set; }
        public string Icao { get; set; } = string.Empty;
        public string Iata { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        [AllowNull]
        public string? City { get; set; }
        [AllowNull]
        public string? State { get; set; }
        public string Country { get; set; } = string.Empty;
        public int Elevation { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }
        public string Tz { get; set; } = string.Empty;
        public List<AirRoute> ArrivalAirRoutes { get; set; } = new List<AirRoute>();
        public List<AirRoute> DepartureAirRoutes { get; set; } = new List<AirRoute>();
    }
}