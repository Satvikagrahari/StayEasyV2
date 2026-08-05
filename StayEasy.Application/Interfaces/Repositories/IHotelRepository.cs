using StayEasy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.Interfaces.Repositories
{
    public interface IHotelRepository
    {
        Task<Hotel?> GetByIdAsync(int id);
        Task<Hotel?> GetByIdWithRoomAsync(int id);
        Task<IEnumerable<Hotel>> GetAllByCityAsync(string? city);
        Task AddAsync(Hotel hotel);
        Task UpdateAsync(Hotel hotel);
        Task DeleteAsync(Hotel hotel);
        Task SaveChangesAsync();

    }
}
