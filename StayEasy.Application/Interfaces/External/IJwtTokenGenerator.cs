using StayEasy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayEasy.Application.Interfaces.External
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
