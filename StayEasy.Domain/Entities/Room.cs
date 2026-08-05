using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Domain.Entities
{
    public class Room
    {
        public Guid RoomId { get; set; }
        public Guid HotelId { get; set; }
        public string RoomType  { get; set; }
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }

    }
}
