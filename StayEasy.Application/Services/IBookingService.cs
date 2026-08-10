using StayEasy.Application.DTOs.Booking;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.Services
{
    public interface IBookingService
    {
        Task<BookingResponseDto> CreateBookingAsync(Guid UserId, CreateBookingDto dto);
        Task<BookingResponseDto> PayBookingAsync(Guid UserId, Guid BookingId, PayBookingDto dto);
    }
}
