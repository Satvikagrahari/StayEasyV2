using StayEasy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(int id);
        Task<IEnumerable<Booking>> GetByUserIdAsync(int userId);
        Task<bool> HasOverLappingBookingAsync(int roomId, DateTime checkIn, DateTime checkOut);
        Task AddAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task SaveChangesAsync();

    }
}
