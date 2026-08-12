using Microsoft.EntityFrameworkCore;
using StayEasy.Application.Interfaces.Repositories;
using StayEasy.Domain.Entities;
using StayEasy.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Infrastructure.Repositories
{
    public class RoomRepository: IRoomRepository
    {
        private readonly StayEasyDbContext _dbContext;
        public RoomRepository(StayEasyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Room?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Rooms.FindAsync(id);
        }
        public async Task<IEnumerable<Room>> GetByHotelIdAsync(Guid hotelId)
        {
            return await _dbContext.Rooms
                .Where(r => r.HotelId == hotelId)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task AddAsync(Room room)
        {
            await _dbContext.Rooms.AddAsync(room);
        }
        public async Task UpdateAsync(Room room)
        {
            _dbContext.Rooms.Update(room);
            await Task.CompletedTask;
        }
        public async Task DeleteAsync(Room room)
        {
            _dbContext.Rooms.Remove(room);
            await Task.CompletedTask;
        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
    
}
