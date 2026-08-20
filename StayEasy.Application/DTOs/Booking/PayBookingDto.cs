using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StayEasy.Application.DTOs.Booking
{
    public record PayBookingDto
    (
        [Required]
        [CreditCard(ErrorMessage = "Invalid credit card number format.")]
        [StringLength(16, MinimumLength = 15)]
        string CardNumber,

        [Required]
        [StringLength(100)]
        string CardHolderName,

        [Required]        
        [RegularExpression("^(0[1-9]|1[0-2])$", ErrorMessage = "Expiration month must be a number between 01 and 12.")]
        string ExpirationMonth,

        [Required]
        [RegularExpression("^20[2-9][0-9]$", ErrorMessage = "Expiration year must be a valid future year (e.g., 2026).")]
        string ExpirationYear,

        [Required]
        [RegularExpression("^[0-9]{3,4}$", ErrorMessage = "CVV must be 3 or 4 digits.")]
        string Cvv
    );
}
