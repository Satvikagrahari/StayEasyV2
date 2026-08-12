using Microsoft.EntityFrameworkCore;
using StayEasy.Application.Interfaces.Repositories;
using StayEasy.Domain.Entities;
using StayEasy.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly StayEasyDbContext _dbContext;
        public BookingRepository(StayEasyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Booking?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Bookings.FindAsync(id);
        }

        public async Task<IEnumerable<Booking>> GetByUserIdAsync(Guid userId)
        {
            return await _dbContext.Bookings.Where(b => b.UserId == userId).AsNoTracking().ToListAsync();
        }
        public async Task<bool> HasOverlappingBookingAsync(Guid roomId, DateTime checkIn, DateTime checkOut)
        {
            return await _dbContext.Bookings.AnyAsync(b => b.RoomId == roomId
                                                      && b.Status == BookingStatus.Confirmed
                                                      && b.CheckIn < checkOut
                                                      && b.CheckOut > checkIn);
        }
        public async Task<int> GetActiveBookingsCountForDatesAsync(Guid roomId, DateTime checkIn, DateTime checkOut)
        {
            // Inventory Check: Count how many non-cancelled bookings overlap with these dates
            return await _dbContext.Bookings
                .CountAsync(b => b.RoomId == roomId
                              && b.Status != BookingStatus.Cancelled
                              && b.CheckIn < checkOut
                              && b.CheckOut > checkIn);
        }
        public async Task AddAsync(Booking booking)
        {
            await _dbContext.Bookings.AddAsync(booking);
        }
        public async Task UpdateAsync(Booking booking)
        {
            _dbContext.Bookings.Update(booking);
            await Task.CompletedTask; //Updates in EF Core are synchronous in memory
        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
