using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.DTOs.Auth
{
    public record UserResponseDto(
        Guid Id,
        string Email,
        string UserName,
        string Role);
}
