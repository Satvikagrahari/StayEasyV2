using Microsoft.EntityFrameworkCore;
using StayEasy.Application.Interfaces.Repositories;
using StayEasy.Domain.Entities;
using StayEasy.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly StayEasyDbContext _dbContext;
        public UserRepository(StayEasyDbContext dbContext)
        {
            _dbContext = dbContext; 
        }
       public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }
        
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            return await _dbContext.Users.FindAsync(userId);
        }

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            return await _dbContext.Users.AsNoTracking().ToListAsync();
            
        }
        public async Task AddAsync(User user)
        {
            // Staging the insertion in memory
            await _dbContext.Users.AddAsync(user);
        }
        public async Task SaveChangesAsync()
        {
            // Actually committing to the database
            await _dbContext.SaveChangesAsync();
        }
    }
}
