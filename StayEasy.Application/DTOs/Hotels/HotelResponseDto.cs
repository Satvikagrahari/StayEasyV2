using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.DTOs.Hotels
{
    public record HotelResponseDto
    (
        int HotelId,
        string Name,
        string Address,
        string City
    );
}
