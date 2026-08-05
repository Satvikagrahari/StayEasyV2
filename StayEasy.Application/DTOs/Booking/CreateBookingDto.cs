using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.DTOs.Booking
{
    public record CreateBookingDto
    (
        int BookingId,
        DateTime CheckIn,
        DateTime CheckOut
    );
}
