using LearnStore.Infrastructure.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Infrastructure.Interfaces
{
    public interface IJwtOptionsProvider
    {
        JwtOptions GetOptions();
    }
}
