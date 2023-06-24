using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using AirLineManager.dal.Entities;

namespace AirlineManager.dal.Entities
{
    public class AircraftModel
    {
        public int AircraftId { get; set; }
        [Required(AllowEmptyStrings = false), NotNull, StringLength(30)] public string Name { get; set; }
        public List<SeatModel>? Seats { get; set; }
    }
}