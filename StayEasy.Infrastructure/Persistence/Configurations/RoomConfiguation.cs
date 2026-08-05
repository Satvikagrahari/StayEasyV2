using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayEasy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Infrastructure.Persistence.Configurations
{
    public class RoomConfiguation : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.ToTable("Rooms");
            builder.HasKey(u => u.RoomId);
            builder.Property(u => u.RoomType).IsRequired().HasMaxLength(100);
            builder.Property(u => u.PricePerNight).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(u => u.Capacity).IsRequired();

        }
    }
}
