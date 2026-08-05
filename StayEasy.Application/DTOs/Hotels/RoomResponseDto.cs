using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.DTOs.Hotels
{
    public record RoomResponseDto
    (
        int Id,
        int HotelId,
        string RoomType,
        decimal PricePerNight,
        int Capacity
    );

}
