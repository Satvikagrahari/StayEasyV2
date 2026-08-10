using StayEasy.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
    }
}
