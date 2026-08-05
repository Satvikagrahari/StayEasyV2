using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayEasy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Infrastructure.Persistence.Configurations
{
    internal class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.ToTable("Hotel");
            builder.HasKey(u => u.HotelId);
            builder.Property(u => u.Name).IsRequired().HasMaxLength(100);
            builder.Property(u => u.City).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Address).IsRequired().HasMaxLength(200);

            builder.HasMany<Room>().WithOne().HasForeignKey(r => r.HotelId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
