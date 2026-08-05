using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.DTOs.Auth
{
    public record AuthResponseDto
    (
        int Id,
        string Email,
        string UserName,
        string Role,
        string Token,
        DateTime ExpiresAt
    );  
}
