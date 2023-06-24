using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AirlineManager.dal.Entities
{
    public class ReservationModel
    {
        public int ReservationId { get; set; }
        public bool IsDeleted { get; set; }
        [Required, NotNull] public int FlightId { get; set; }
        public List<ReservedSeatModel>? ReservedSeats { get; set; }
        public string UserId { get; set; }
    }
}