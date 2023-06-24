using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using AirLineManager.dal.Entities;

namespace AirlineManager.dal.Entities
{
    public class Aircraft
    {
        public int AircraftId { get; set; }
        [Required, NotNull, StringLength(30)] public string Name { get; set; } = string.Empty;
        public List<Seat> Seats { get; set; } = new List<Seat>();
        public List<AirRoute> AirRoutes { get; set; } = new List<AirRoute>();
    }
}