using StayEasy.Application.DTOs.Booking;
using StayEasy.Application.Interfaces.External;
using StayEasy.Application.Interfaces.Repositories;
using StayEasy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
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

            int nights = (dto.CheckOut - dto.CheckIn).Days;
            decimal totalPrice = nights * room.PricePerNight;
            var booking = new Booking
            {
                UserId = userId,
                RoomId = dto.RoomId,
                CheckIn = dto.CheckIn,
                CheckOut = dto.CheckOut,
                TotalPrice = totalPrice,
                Status = BookingStatus.Pending
            };
            await _bookingRepo.AddAsync(booking);
            await _bookingRepo.SaveChangesAsync();
            return new BookingResponseDto(
                booking.BookingId,
                booking.RoomId,
                booking.CheckIn,
                booking.CheckOut,
                booking.TotalPrice,
                booking.Status.ToString());
        }
        public async Task<BookingResponseDto> PayBookingAsync(Guid userId, Guid bookingId, PayBookingDto dto)
        {
            var booking = await _bookingRepo.GetByIdAsync(bookingId) ?? throw new KeyNotFoundException("Booking Not Found");

            if (booking.UserId != userId)
            {
                throw new UnauthorizedAccessException("Cannot pay for someone else's booking.");
            }

            if(booking.Status != BookingStatus.Pending)
            {
                throw new InvalidOperationException("Booking is not in pending payment status.");
            }

            var payResult = await _paymentService.ProcessPaymentAsync(new PaymentRequestDto(booking.BookingId, booking.TotalPrice, dto.CardNumber, dto.CardHolderName));

            if (!payResult.IsSuccess)
            {
                throw new InvalidOperationException($"Payment failed : {payResult.ErrorMessage}");
            }

            booking.Status = BookingStatus.Confirmed;
            booking.PaymentTransactionId = payResult.TransactionId;

            await _bookingRepo.UpdateAsync(booking);
            await _bookingRepo.SaveChangesAsync();

            return new BookingResponseDto(
                booking.BookingId,
                booking.RoomId,
                booking.CheckIn,
                booking.CheckOut,
                booking.TotalPrice,
                booking.Status.ToString(),
                booking.PaymentTransactionId
                );
        }

        public async Task<IEnumerable<BookingResponseDto>> GetBookingsByDateRangeAsync(DateTime fromDate, DateTime endDate)
        {
            var bookings = await _bookingRepo.GetBookingsByDateRangeAsync(fromDate, endDate);
            return bookings.Select(booking => new BookingResponseDto(
                booking.BookingId,
                booking.RoomId,
                booking.CheckIn,
                booking.CheckOut,
                booking.TotalPrice,
                booking.Status.ToString(),
                booking.PaymentTransactionId));
        }

        public async Task<BookingResponseDto> GetBookingByIdAsync(Guid bookingId)
        {
            var booking = await _bookingRepo.GetByIdAsync(bookingId) ?? throw new KeyNotFoundException("Booking not found.");
            return new BookingResponseDto(
                booking.BookingId,
                booking.RoomId,
                booking.CheckIn,
                booking.CheckOut,
                booking.TotalPrice,
                booking.Status.ToString(),
                booking.PaymentTransactionId);
        }

        public async Task<IEnumerable<BookingResponseDto>> GetMyBookingsAsync(Guid userId)
        {
            var booking = await _bookingRepo.GetByUserIdAsync(userId);
            return booking.Select(b=> new BookingResponseDto(b.BookingId, b.RoomId, b.CheckIn, b.CheckOut, b.TotalPrice,
                b.Status.ToString(), b.PaymentTransactionId));
        }

        public async Task<BookingResponseDto> CancelBookingAsync(Guid userId, Guid bookingId)
        {
            var booking = await _bookingRepo.GetByIdAsync(bookingId) ?? throw new KeyNotFoundException("Booking not found");

            if (booking.UserId != userId) throw new UnauthorizedAccessException("You cannot cancel someone else's booking.");

            if (booking.Status == BookingStatus.Cancelled) throw new InvalidOperationException("This booking is already cancelled.");

            if (booking.CheckIn > DateTime.UtcNow) throw new InvalidOperationException("You cannot cancel a booking after the check-in date has passed.");

            booking.Status = BookingStatus.Cancelled;
            await _bookingRepo.UpdateAsync(booking);
            await _bookingRepo.SaveChangesAsync();
            return new BookingResponseDto(booking.BookingId, booking.RoomId, booking.CheckIn, booking.CheckOut,
                booking.TotalPrice, booking.Status.ToString(), booking.PaymentTransactionId);
        }
    }
}
