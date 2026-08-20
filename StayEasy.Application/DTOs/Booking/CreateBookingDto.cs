using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StayEasy.Application.DTOs.Booking
{
    public record CreateBookingDto
    (
        [Required]
        Guid RoomId,
        [Required]
        DateTime CheckIn,
        [Required]
        DateTime CheckOut
    );
}
