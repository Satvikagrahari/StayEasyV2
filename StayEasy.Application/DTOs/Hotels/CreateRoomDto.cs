using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StayEasy.Application.DTOs.Hotels
{
    public record CreateRoomDto
    (
        [Required]
        string RoomType,

        [Required]
        decimal PricePerNight,

        int Capacity,

        [Required]
        int TotalRooms
    );
}
