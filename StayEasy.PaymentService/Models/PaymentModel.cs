namespace StayEasy.PaymentService.Models
{
    public record PaymentRequest(
    Guid BookingId, 
    decimal Amount,
    string CardNumber,
    string CardHolderName
);
    public record PaymentResponse(
        bool IsSuccess,
        string? TransactionId,
        string? ErrorMessage
    );
}
