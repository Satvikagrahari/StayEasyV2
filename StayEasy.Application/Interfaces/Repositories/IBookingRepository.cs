using StayEasy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(Guid id);
        Task<IEnumerable<Booking>> GetByUserIdAsync(Guid userId);
        Task<int> GetActiveBookingsCountForDatesAsync(Guid roomId, DateTime checkIn, DateTime checkOut);
        Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime fromDate, DateTime endDate);
        Task AddAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task SaveChangesAsync();

    }
}
