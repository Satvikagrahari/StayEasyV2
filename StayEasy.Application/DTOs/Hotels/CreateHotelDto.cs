using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StayEasy.Application.DTOs.Hotels
{
    public record CreateHotelDto
    (
        [Required]
        [StringLength(100, ErrorMessage = "Hotel name cannot exceed 100 characters.")]
        string Name,

        [Required]
        [StringLength(200)]
        string Address,

        [Required]
        [StringLength(50)]
        string City

    );
}
