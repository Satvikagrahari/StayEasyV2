using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayEasy.Application.DTOs.Booking;
using StayEasy.Application.Services;
using System.Security.Claims;

namespace StayEasy.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(CreateBookingDto request)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out Guid userId)) return Unauthorized();
            var response = await _bookingService.CreateBookingAsync(userId, request);
            return Ok(response);
        }

        [HttpPost("{bookingId:guid}/pay")]
        public async Task<IActionResult> PayForBooking(Guid bookingId, PayBookingDto request)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out Guid userId)) return Unauthorized();
            var response = await _bookingService.PayBookingAsync(userId, bookingId, request);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("range")]
        public async Task<IActionResult> GetBookingsByDateRange([FromQuery] DateTime fromDate, [FromQuery] DateTime endDate)
        {
            if(fromDate>= endDate)
            {
                return BadRequest("The 'fromDate' must be earlier than the 'endDate'.");
            }

            var bookings = await _bookingService.GetBookingsByDateRangeAsync(fromDate, endDate);
            return Ok(bookings);
        }

        [Authorize]
        [HttpGet("{bookingId:guid}")]
        public async Task<IActionResult> GetBookingById(Guid bookingId)
        {
            var booking = await _bookingService.GetBookingByIdAsync(bookingId);
            return Ok(booking);
        }

        [Authorize]
        [HttpGet("my-Bookings")]
        public async Task<IActionResult> GetMyBookings()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out Guid userId)) return Unauthorized();

            var bookings = await _bookingService.GetMyBookingsAsync(userId);
            return Ok(bookings);
        }

        [Authorize]
        [HttpPut("{bookingId:guid}/cancel")]
        public async Task<IActionResult> CancelBooking(Guid bookingId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out Guid userId)) return Unauthorized();
            var response = await _bookingService.CancelBookingAsync(userId, bookingId);
            return Ok(response);
        }
    }
}
