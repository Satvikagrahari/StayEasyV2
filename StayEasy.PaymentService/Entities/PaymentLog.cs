using System.ComponentModel.DataAnnotations;

namespace StayEasy.PaymentService.Entities
{
    public class PaymentLog
    {
        [Key]
        public Guid LogId { get; set; } = Guid.NewGuid();
        public Guid BookingId { get; set; }
        public decimal Amount { get; set; }
        public string CardHolderName { get; set; } = string.Empty;
        public string MaskedCardNumber { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public string? TransactionId { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
