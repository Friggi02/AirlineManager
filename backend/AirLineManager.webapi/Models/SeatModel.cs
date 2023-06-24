using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AirLineManager.dal.Entities
{
    public class SeatModel
    {
        public int SeatId { get; set; }
        [Required, NotNull] public int AircraftId { get; set; }
        [Required, NotNull, StringLength(3)] public string Code { get; set; }
        [Required, NotNull] public byte Row { get; set; }
        [Required, NotNull] public char Column { get; set; }
        [Required, NotNull] public Category Category { get; set; }
    }
}