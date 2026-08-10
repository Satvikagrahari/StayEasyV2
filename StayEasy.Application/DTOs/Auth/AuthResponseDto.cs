using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.DTOs.Auth
{
    public record AuthResponseDto
    (
        Guid Id,
        string Email,
        string UserName,
        string Role,
        string Token,
        DateTime ExpiresAt
    );  
}
