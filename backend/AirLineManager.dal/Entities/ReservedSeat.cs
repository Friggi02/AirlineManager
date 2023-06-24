using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using AirLineManager.dal.Entities;

namespace AirlineManager.dal.Entities
{
    public class ReservedSeat
    {
        [Required, NotNull, StringLength(3)] public string SeatCode { get; set; }
        [Required, NotNull, StringLength(50)] public string PassengerName { get; set; }
        [Required, NotNull, StringLength(50)] public string PassengerSurname { get; set; }
        [Required, NotNull] public byte PassengerAge { get; set; }
        [Required, NotNull] public int ReservationId { get; set; }
        [Required, NotNull] public int SeatId { get; set; }
        public bool IsDeleted { get; set; }
        public Reservation Reservation { get; set; }
        public Seat Seat { get; set; }
    }
}