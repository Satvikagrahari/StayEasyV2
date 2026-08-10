using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.DTOs.Booking
{
    public record CreateBookingDto
    (
        Guid RoomId,
        DateTime CheckIn,
        DateTime CheckOut
    );
}
