using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.DTOs.Booking
{
    public record PayBookingDto
    (
        string CardNumber,
        string CardHolderName,
        string ExpirationMonth,
        string ExpirationYear,
        string Cvv
    );
}
