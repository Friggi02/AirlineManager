namespace AirLineManager.webapi.Models
{
    public class JourneyModel
    {
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public DateTime StartPeriod { get; set; }
        public DateTime? EndPeriod { get; set; }
    }
}