using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.DTOs.Hotels
{
    public record CreateHotelDto
    (
        string Name,
        string Address,
        string City

    );
}
