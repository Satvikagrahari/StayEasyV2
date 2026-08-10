using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.DTOs.Hotels
{
    public record HotelResponseDto
    (
        Guid HotelId,
        string Name,
        string Address,
        string City
    );
}
