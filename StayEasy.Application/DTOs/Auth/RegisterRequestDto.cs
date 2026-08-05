using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.DTOs.Auth
{
    public record RegisterRequestDto
    (
        string Email,
        string Password,
        string UserName,
        string? Role = "Customer"
    );
}
