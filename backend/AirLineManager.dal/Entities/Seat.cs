using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using AirlineManager.dal.Entities;

namespace AirLineManager.dal.Entities
{
    public class Seat
    {
        public int SeatId { get; set; }
        [Required, NotNull] public int AircraftId { get; set; }
        [Required, NotNull, StringLength(3)] public string Code { get; set; }
        [Required, NotNull] public byte Row { get; set; }
        [Required, NotNull] public char Column { get; set; }
        [Required, NotNull] public Category Category { get; set; }
        public Aircraft Aircraft { get; set; }
    }

    public enum Category : byte
    {
        Business = 0,
        Economy = 1,
    }
}
