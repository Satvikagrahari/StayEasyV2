using Moq;
using StayEasy.Application.Interfaces.External;
using StayEasy.Application.Interfaces.Repositories;
using StayEasy.Application.Services;
using StayEasy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Test
{
    [TestFixture]
    public class BookingServiceTests
    {
       
        private Mock<IBookingRepository> _mockBookingRepo;
        private Mock<IRoomRepository> _mockRoomRepo;
        private Mock<IPaymentService> _mockPaymentService;

        private BookingService _bookingService;       
        [SetUp]
        public void Setup()
        {
            _mockBookingRepo = new Mock<IBookingRepository>();
            _mockRoomRepo = new Mock<IRoomRepository>();
            _mockPaymentService = new Mock<IPaymentService>();            
            _bookingService = new BookingService(
                _mockBookingRepo.Object,
                _mockRoomRepo.Object,
                _mockPaymentService.Object
            );
        }
        [Test]
        public void CancelBookingAsync_ShouldThrowUnauthorized_WhenUserIsNotOwner()
        {
            // 1. ARRANGE: Set up our test data and fake repository responses
            var currentUserId = Guid.NewGuid();
            var differentUserId = Guid.NewGuid();
            var bookingId = Guid.NewGuid();
            var fakeDatabaseBooking = new Booking
            {
                BookingId = bookingId,
                UserId = differentUserId, // Belonging to someone else!
                Status = BookingStatus.Pending
            };
            // Tell the fake repo: "When someone asks for this ID, return this fake booking"
            _mockBookingRepo.Setup(repo => repo.GetByIdAsync(bookingId))
                            .ReturnsAsync(fakeDatabaseBooking);
            // 2. ACT & 3. ASSERT: Try to cancel it and verify it throws the correct exception
            var exception = Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
                await _bookingService.CancelBookingAsync(currentUserId, bookingId));

            Assert.That(exception.Message, Is.EqualTo("You cannot cancel someone else's booking."));
        }
        [Test]
        public async Task CancelBookingAsync_ShouldChangeStatusToCancelled_WhenSuccessful()
        {
            // 1. ARRANGE
            var userId = Guid.NewGuid();
            var bookingId = Guid.NewGuid();
            var fakeDatabaseBooking = new Booking
            {
                BookingId = bookingId,
                UserId = userId, // Belongs to the current user
                Status = BookingStatus.Pending,
                CheckIn = DateTime.UtcNow.AddDays(5) 
            };
            _mockBookingRepo.Setup(repo => repo.GetByIdAsync(bookingId))
                            .ReturnsAsync(fakeDatabaseBooking);
            // 2. ACT
            var result = await _bookingService.CancelBookingAsync(userId, bookingId);
            // 3. ASSERT
            Assert.That(result.Status, Is.EqualTo("Cancelled"));

            // Verify that UpdateAsync was actually called exactly once to save the changes
            _mockBookingRepo.Verify(repo => repo.UpdateAsync(It.IsAny<Booking>()), Times.Once);
            _mockBookingRepo.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void CreateBookingAsync_ShouldThrowException_WhenCheckOutIsBeforeCheckIn()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var dto = new StayEasy.Application.DTOs.Booking.CreateBookingDto(
                Guid.NewGuid(),
                DateTime.UtcNow.AddDays(5), // Check In
                DateTime.UtcNow.AddDays(2)  // Check Out (Invalid - before check in!)
            );

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _bookingService.CreateBookingAsync(userId, dto));

            Assert.That(ex.Message, Is.EqualTo("Check-out date must be after check-in date"));
        }

        [Test]
        public void CreateBookingAsync_ShouldThrowException_WhenRoomIsSoldOut()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var roomId = Guid.NewGuid();
            var checkIn = DateTime.UtcNow.AddDays(1);
            var checkOut = DateTime.UtcNow.AddDays(3);

            var dto = new StayEasy.Application.DTOs.Booking.CreateBookingDto(roomId, checkIn, checkOut);

            // Fake a room that only has 2 total capacity
            var room = new Room { RoomId = roomId, TotalRooms = 2, PricePerNight = 1000 };
            _mockRoomRepo.Setup(r => r.GetByIdAsync(roomId)).ReturnsAsync(room);

            // Fake that there are ALREADY 2 active bookings for these dates
            _mockBookingRepo.Setup(r => r.GetActiveBookingsCountForDatesAsync(roomId, checkIn, checkOut))
                            .ReturnsAsync(2);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _bookingService.CreateBookingAsync(userId, dto));

            Assert.That(ex.Message, Is.EqualTo("Sorry, this room type is completely sold out for these dates."));
        }
    }

}
