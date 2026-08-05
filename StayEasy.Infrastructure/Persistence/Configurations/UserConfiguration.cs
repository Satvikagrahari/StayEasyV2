using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayEasy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //table name
            builder.ToTable("Users");

            //primary key
            builder.HasKey(u => u.UserId);

            //required fields and max length
            builder.Property(u => u.Email).IsRequired().HasMaxLength(150);

            //check for unique email
            builder.HasIndex(u => u.Email).IsUnique();

            //some other required fields and max lenght
            builder.Property(u => u.UserName).IsRequired().HasMaxLength(100);
            builder.Property(u => u.PasswordHash).IsRequired(); ;
            builder.Property(u => u.Role).IsRequired().HasMaxLength(20);

        }
    }
}
