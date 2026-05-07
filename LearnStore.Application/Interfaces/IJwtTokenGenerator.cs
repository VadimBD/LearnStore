using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(UserDto user);
    }
}
