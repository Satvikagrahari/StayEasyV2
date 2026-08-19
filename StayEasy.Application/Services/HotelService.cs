using StayEasy.Application.DTOs.Hotels;
using StayEasy.Application.Interfaces.Repositories;
using StayEasy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IRoomRepository _roomRepository;

        public HotelService(IHotelRepository hotelRepository, IRoomRepository roomRepository)
        {
            _hotelRepository = hotelRepository;
            _roomRepository = roomRepository;
        }

        public async Task<HotelResponseDto> CreateHotelAsync(CreateHotelDto dto)
        {
            var hotel = new Hotel
            {
                Name = dto.Name,
                Address = dto.Address,
                City = dto.City         
            };
 
            await _hotelRepository.AddAsync(hotel);
            await _hotelRepository.SaveChangesAsync();
            return new HotelResponseDto(hotel.HotelId, hotel.Name, hotel.Address, hotel.City);
        }

        public async Task<RoomResponseDto> AddRoomAsync(Guid hotelId ,CreateRoomDto dto)
        {
            var hotel = await _hotelRepository.GetByIdAsync(hotelId);
            if(hotel == null)
            {
                throw new KeyNotFoundException("Hotel not found");
            }
            var room = new Room
            {
                HotelId = hotelId,
                RoomType = dto.RoomType,
                PricePerNight = dto.PricePerNight,
                Capacity = dto.Capacity,
                TotalRooms = dto.TotalRooms
            };
            await _roomRepository.AddAsync(room);
            await _roomRepository.SaveChangesAsync();
            return new RoomResponseDto(room.RoomId, room.HotelId, room.RoomType, room.PricePerNight, room.Capacity, room.TotalRooms);
        }

        public async Task<HotelResponseDto> GetHotelByIdAsync(Guid hotelId)
        {
            var hotel = await _hotelRepository.GetByIdWithRoomAsync(hotelId) ?? throw new KeyNotFoundException("Hotel not found.");
            return new HotelResponseDto(
                hotel.HotelId,
                hotel.Name,
                hotel.Address,
                hotel.City,
                hotel.Rooms?.Select(r => new RoomResponseDto(r.RoomId, r.HotelId, r.RoomType, r.PricePerNight, r.Capacity, r.TotalRooms)).ToList()
            );
        }
        public async Task<IEnumerable<HotelResponseDto>> GetAllHotelsAsync(string? city)
        {
            var hotels = await _hotelRepository.GetAllByCityAsync(city);

            // Convert Domain Entities to DTOs using LINQ
            return hotels.Select(h => new HotelResponseDto(
                h.HotelId, h.Name, h.City, h.Address
            ));
        }

        public async Task<IEnumerable<HotelResponseDto>> GetAllHotelWithRoomsAsync()
        {
            var hotels = await _hotelRepository.GetAllWithRoomsAsync();
            return hotels.Select(h => new HotelResponseDto(h.HotelId, h.Name, h.Address, h.City,
                h.Rooms?.Select(r=> new RoomResponseDto(r.RoomId,r.HotelId, r.RoomType, r.PricePerNight,r.Capacity, r.TotalRooms)).ToList()
                ));
        }

        public async Task UpdateHotelAsync(Guid hotelId, UpdateHotelDto dto)
        {
            var hotel = await _hotelRepository.GetByIdAsync(hotelId) ?? throw new KeyNotFoundException("Hotel not found.");

            hotel.Name = dto.Name;
            hotel.Address = dto.Address;
            hotel.City = dto.City;

            await _hotelRepository.UpdateAsync(hotel);
            await _hotelRepository.SaveChangesAsync();
        }

        public async Task DeleteHotelAsync(Guid hotelId)
        {
            var hotel = await _hotelRepository.GetByIdAsync(hotelId) ?? throw new KeyNotFoundException("Hotel not found.");
            await _hotelRepository.DeleteAsync(hotel);
            await _hotelRepository.SaveChangesAsync();
        }

        public async Task UpdateRoomAsync(Guid roomId, UpdateRoomDto dto)
        {
            var room = await _roomRepository.GetByIdAsync(roomId) ?? throw new KeyNotFoundException("Room not found.");

            room.RoomType = dto.RoomType;
            room.PricePerNight = dto.PricePerNight;
            room.Capacity = dto.Capacity;
            room.TotalRooms = dto.TotalRooms;

            await _roomRepository.UpdateAsync(room);
            await _roomRepository.SaveChangesAsync();
        }
        public async Task DeleteRoomAsync(Guid roomId)
        {
            var room = await _roomRepository.GetByIdAsync(roomId) ?? throw new KeyNotFoundException("Room not found.");
            await _roomRepository.DeleteAsync(room);
            await _roomRepository.SaveChangesAsync();
        }
    }
}
