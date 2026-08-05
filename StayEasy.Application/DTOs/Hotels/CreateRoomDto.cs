using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.DTOs.Hotels
{
    public record CreateRoomDto
    (        
        string RoomType,
        decimal PricePerNight,
        int Capacity
    );
}
