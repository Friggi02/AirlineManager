namespace AirLineManager.webapi.Models
{
    public class DuplicateFlightsModel
    {
        public DateTime Day { get; set; }
        public DateTime StartPeriod { get; set; }
        public DateTime EndPeriod { get; set; }
    }
}
