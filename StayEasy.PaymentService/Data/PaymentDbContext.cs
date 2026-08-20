using Microsoft.EntityFrameworkCore;
using StayEasy.PaymentService.Entities;

namespace StayEasy.PaymentService.Data
{
    public class PaymentDbContext: DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
        {            
        }

        public DbSet<PaymentLog> PaymentLogs { get; set; }

    }
}
