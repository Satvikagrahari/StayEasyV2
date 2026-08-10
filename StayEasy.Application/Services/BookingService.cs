using StayEasy.Application.DTOs.Booking;
using StayEasy.Application.Interfaces.External;
using StayEasy.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly IRoomRepository _roomRepo;
        private readonly IPaymentService _paymentService;

        public BookingService(IBookingRepository bookingRepo, IRoomRepository roomRepo, IPaymentService paymentService)
        {
            _bookingRepo = bookingRepo;
            _roomRepo = roomRepo;
            _paymentService = paymentService;
        }
        public async Task<BookingResponseDto> CreateBookingAsync(Guid userId, CreateBookingDto dto)
        {
            if (dto.CheckIn >= dto.CheckOut)
            {
                throw new ArgumentException("Check-out date must be after check-in date");
            }
            var room = await _roomRepo.GetByIdAsync(dto.RoomId) ?? throw new KeyNotFoundException("Room not found.");
            int overlappingBookings = await _bookingRepo.GetActiveBookingsCountForDatesAsync(
       dto.RoomId, dto.CheckIn, dto.CheckOut);
            
            if (overlappingBookings >= room.TotalRooms)
            {
                throw new InvalidOperationException("Sorry, this room type is completely sold out for these dates.");
            }

        }
    }
}
