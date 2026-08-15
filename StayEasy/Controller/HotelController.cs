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
        [HttpPost("{hotel:guid}/rooms")]
        public async Task<IActionResult> AddRoomToHotel(Guid hotelId, CreateRoomDto request)
        {
            var response = await _hotelService.AddRoomAsync(hotelId,request);
            return Ok(response);
        }

    }
}
