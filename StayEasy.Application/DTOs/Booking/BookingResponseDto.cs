using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.DTOs.Booking
{
    public record BookingResponseDto
    (
        int Id,
        int RoomId,
        DateTime CheckInDate,
        DateTime CheckOutDate,
        decimal TotalPrice,
        string Status,
        string? PaymentTransactionId = null

    );
}
