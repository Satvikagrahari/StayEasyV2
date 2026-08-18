using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.DTOs.Hotels
{
    public record UpdateRoomDto
    (
        string RoomType,
        decimal PricePerNight,
        int Capacity,
        int TotalRooms
    );
    
}
