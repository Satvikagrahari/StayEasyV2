using StayEasy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.Interfaces.Repositories
{
    public interface IHotelRepository
    {
        Task<Hotel?> GetByIdAsync(Guid id);
        Task<IEnumerable<Hotel>> GetAllWithRoomsAsync();
        Task<Hotel?> GetByIdWithRoomAsync(Guid id);
        Task<IEnumerable<Hotel>> GetAllByCityAsync(string? city);
        Task AddAsync(Hotel hotel);
        Task UpdateAsync(Hotel hotel);
        Task DeleteAsync(Hotel hotel);
        Task SaveChangesAsync();

    }
}
