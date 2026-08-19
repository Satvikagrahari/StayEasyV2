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
        Task<HotelResponseDto> GetHotelByIdAsync(Guid hotelId);
        Task<IEnumerable<HotelResponseDto>> GetAllHotelsAsync(string? city);
        Task<IEnumerable<HotelResponseDto>> GetAllHotelWithRoomsAsync();
        Task UpdateHotelAsync(Guid hotelId, UpdateHotelDto dto);
        Task DeleteHotelAsync(Guid hotelId);
        Task UpdateRoomAsync(Guid RoomId, UpdateRoomDto dto);
        Task DeleteRoomAsync(Guid RoomId);
    }
}
