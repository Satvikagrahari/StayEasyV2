using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StayEasy.Application.DTOs.Auth
{
    public record LoginRequestDto
    (
        [Required]
        [EmailAddress]
        string Email,
        [Required]
        string Password
    );
}
