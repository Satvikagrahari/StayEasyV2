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
    }
}
