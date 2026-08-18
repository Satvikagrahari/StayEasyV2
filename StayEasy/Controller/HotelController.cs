using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayEasy.Application.DTOs.Hotels;
using StayEasy.Application.Services;

namespace StayEasy.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet("with-rooms")]
        public async Task<IActionResult> GetHotelsWithRoomsAsync()
        {
            var hotels = await _hotelService.GetAllHotelWithRoomsAsync();
            return Ok(hotels);
        }

        [HttpGet]
        public async Task<IActionResult> GetHotels([FromQuery] string? city)
        {
            var hotels = await _hotelService.GetAllHotelsAsync(city);
            return Ok(hotels);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddHotel(CreateHotelDto request)
        {
            var response = await _hotelService.CreateHotelAsync(request);
            return Created($"/api/hotels/{response.HotelId}", response);
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("{hotelId:guid}/rooms")]
        public async Task<IActionResult> AddRoomToHotel(Guid hotelId, CreateRoomDto request)
        {
            var response = await _hotelService.AddRoomAsync(hotelId,request);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{hotelId:guid}")]
        public async Task<IActionResult> UpdateHotelAsync(Guid hotelId, UpdateHotelDto request)
        {
            await _hotelService.UpdateHotelAsync(hotelId, request);
            return NoContent();

        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{hotelId:guid}")]
        public async Task<IActionResult> DeleteHotelAsync(Guid hotelId)
        {
            await _hotelService.DeleteHotelAsync(hotelId);
            return NoContent();
        }

        [Authorize(Roles="Admin")]
        [HttpPut("{hotelId:guid}/rooms/{roomId:guid}")]
        public async Task<IActionResult> UpdateRoomToHotelAsync(Guid hotelId, Guid roomId, UpdateRoomDto request)
        {
            await _hotelService.UpdateRoomAsync(roomId, request);
            return NoContent();
        }

        [Authorize(Roles ="Admin")]
        [HttpDelete("{hotelId:guid}/rooms/{roomId:guid}")]
        public async Task<IActionResult> DeleteRoomToHotelAsync(Guid hotelId, Guid roomId)
        {
            await _hotelService.DeleteRoomAsync(roomId);
            return NoContent();
        }
    }
}
