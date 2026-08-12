using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayEasy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Infrastructure.Persistence.Configurations
{
    public class BookingConfiguration:IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Bookings");
            builder.HasKey(u => u.BookingId);
            builder.Property(u => u.CheckIn).IsRequired();
            builder.Property(u => u.CheckOut).IsRequired();
            builder.Property(u => u.TotalPrice).IsRequired().HasColumnType("decimal(18,2)");
            //Store the Enum as a readable String in the database instead of an Integer (0, 1, 2)
            builder.Property(b => b.Status).IsRequired().HasConversion<string>().HasMaxLength(30);
            builder.Property(b => b.PaymentTransactionId).HasMaxLength(100);
        }
    }
}
