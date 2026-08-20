using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StayEasy.Application.DTOs.Auth
{
    public record RegisterRequestDto
    (
        [Required(ErrorMessage ="Email is required.")]
        [EmailAddress(ErrorMessage ="Invalid email format.")]
        string Email,

        [Required(ErrorMessage ="Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        string Password,

        [Required(ErrorMessage ="Username is required.")]
        [StringLength(50,MinimumLength =3,ErrorMessage ="Username must be between 3 and 50 characters.")]
        string UserName        
    );
}
