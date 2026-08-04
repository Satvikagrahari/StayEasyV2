using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Domain.Entities
{
    public class Hotel
    {
        public Guid HotelId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
      
    }
}
