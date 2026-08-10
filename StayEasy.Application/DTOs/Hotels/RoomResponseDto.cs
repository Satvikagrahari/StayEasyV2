using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.DTOs.Hotels
{
    public record RoomResponseDto
    (
        Guid Id,
        Guid HotelId,
        string RoomType,
        decimal PricePerNight,
        int Capacity
    );

}
