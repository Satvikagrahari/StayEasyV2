using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.Interfaces.External
{
    public record PaymentRequestDto(
        Guid BookingId,
        decimal Amount,
        string CardNumber,
        string CardHolderName);

    public record PaymentResponseDto(
        bool IsSuccess,
        string? TransactionId,
        string? ErrorMessage
        );
    public interface IPaymentService
    {
        Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto request);
    }
}
