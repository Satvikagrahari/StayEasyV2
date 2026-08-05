using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.DTOs.Auth
{
    public record LoginRequestDto
    (
        string Email,
        string Password
    );
}
