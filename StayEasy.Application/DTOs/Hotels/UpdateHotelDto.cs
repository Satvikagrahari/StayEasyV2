using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.DTOs.Hotels
{
    public record UpdateHotelDto
    (
        string Name,
        string Address,
        string City
    );
}
