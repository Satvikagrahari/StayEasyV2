using StayEasy.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using StayEasy.Application.Interfaces.External;
using StayEasy.Application.DTOs.Auth;
using StayEasy.Domain.Entities;

namespace StayEasy.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
        {
            if (await _userRepository.ExistsByEmailAsync(dto.Email))
            {
                throw new InvalidOperationException("Email is already in use");
            }

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                UserName = dto.UserName,
                Role = "Customer"               
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var Token = _jwtTokenGenerator.GenerateToken(user);
            return new AuthResponseDto
            (
                user.UserId,
                user.Email,
                user.PasswordHash,
                user.Role,
                Token,
                DateTime.UtcNow.AddHours(2)
             );
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }
            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthResponseDto(
            user.UserId,
            user.Email,
            user.UserName,            
            user.Role,
            token,
            DateTime.UtcNow.AddHours(2)
        );

        }
    }
}
