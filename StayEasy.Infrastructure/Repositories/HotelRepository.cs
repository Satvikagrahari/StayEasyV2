using Microsoft.EntityFrameworkCore;
using StayEasy.Application.Interfaces.Repositories;
using StayEasy.Domain.Entities;
using StayEasy.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Infrastructure.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly StayEasyDbContext _dbContext;
        public HotelRepository(StayEasyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Hotel?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Hotels.FindAsync(id);
        }
        public async Task<IEnumerable<Hotel>> GetAllWithRoomsAsync()
        {
            return await _dbContext.Hotels.Include(h => h.Rooms).AsNoTracking().ToListAsync();
        }
        public async Task<Hotel?> GetByIdWithRoomAsync(Guid id)
        {
            return await _dbContext.Hotels
                .Include(h => h.Rooms)
                .FirstOrDefaultAsync(h => h.HotelId == id);

        }
        public async Task<IEnumerable<Hotel>> GetAllByCityAsync(string? city)
        {
            var query = _dbContext.Hotels.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(city))
            {
                query = query.Where(h => h.City.ToLower() == city.ToLower());
            }
            return await query.ToListAsync();
        }
        public async Task AddAsync(Hotel hotel)
        {
            await _dbContext.Hotels.AddAsync(hotel);
        }
        public async Task UpdateAsync(Hotel hotel)
        {
            _dbContext.Hotels.Update(hotel);
            await Task.CompletedTask; // Update in EF Core is synchronous
        }
        public async Task DeleteAsync(Hotel hotel)
        {
            _dbContext.Hotels.Remove(hotel);
            await Task.CompletedTask;

        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
