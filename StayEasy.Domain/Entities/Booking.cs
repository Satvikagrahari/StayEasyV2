using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Domain.Entities
{
    public class Booking
    {
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public string? PaymentTransactionId { get; set; }
    }
}
