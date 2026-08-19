using StayEasy.Application.DTOs.Booking;
using StayEasy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.Services
{
    public interface IBookingService
    {
        Task<BookingResponseDto> CreateBookingAsync(Guid UserId, CreateBookingDto dto);
        Task<BookingResponseDto> PayBookingAsync(Guid UserId, Guid BookingId, PayBookingDto dto);
        Task<IEnumerable<BookingResponseDto>> GetBookingsByDateRangeAsync(DateTime fromDate, DateTime endDate);
        Task<BookingResponseDto> GetBookingByIdAsync(Guid bookingId);
        Task<IEnumerable<BookingResponseDto>> GetMyBookingsAsync(Guid userId);
        Task<BookingResponseDto> CancelBookingAsync(Guid userId, Guid bookingId);
    }
}
