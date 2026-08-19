using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.DTOs.Booking
{
    public record BookingResponseDto
    (
        Guid Id,
        //Guid HotelId,
        Guid RoomId,
        DateTime CheckInDate,
        DateTime CheckOutDate,
        decimal TotalPrice,
        string Status,
        string? PaymentTransactionId = null

    );
}
