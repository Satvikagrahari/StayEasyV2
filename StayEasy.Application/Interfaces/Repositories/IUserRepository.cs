using StayEasy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<bool> ExistsByEmailAsync(string email);
        Task<User?> GetByEmailIdAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task SaveChangesAsync();
    }
}
