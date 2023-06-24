using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using AirLineManager.dal.Entities;

namespace AirlineManager.dal.Entities
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        [Required, NotNull] public int FlightId { get; set; }
        public bool IsDeleted { get; set; }
        public Flight Flight { get; set; }
        public List<ReservedSeat> ReservedSeats { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}