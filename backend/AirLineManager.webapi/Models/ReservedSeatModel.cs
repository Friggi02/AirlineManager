﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AirlineManager.dal.Entities
{
    public class ReservedSeatModel
    {
        public int ReservedSeatId { get; set; }
        [Required(AllowEmptyStrings = false), NotNull, StringLength(3)] public string SeatCode { get; set; }
        [Required(AllowEmptyStrings = false), NotNull, StringLength(50)] public string PassengerName { get; set; }
        [Required(AllowEmptyStrings = false), NotNull, StringLength(50)] public string PassengerSurname { get; set; }
        [Required, NotNull] public byte PassengerAge { get; set; }
        [Required, NotNull] public int ReservationId { get; set; }
        public bool IsDeleted { get; set; }
        public int SeatId { get; set; }
    }
}