using StayEasy.Application.DTOs.Hotels;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.Services
{
    public interface IHotelService
    {
        Task<HotelResponseDto> CreateHotelAsync(CreateHotelDto dto);
        Task<RoomResponseDto> AddRoomAsync(Guid hotelId, CreateRoomDto dto);
        Task<IEnumerable<HotelResponseDto>> GetAllHotelsAsync(string? city);

    }
}
