using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using StayEasy.Domain.Entities;
using System.Reflection;


namespace StayEasy.Infrastructure.Persistence
{
    public class StayEasyDbContext : DbContext
    {
        public StayEasyDbContext(DbContextOptions<StayEasyDbContext> options): base(options)
        {
        }
        public DbSet<User> User { get; set; } 
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Booking { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
