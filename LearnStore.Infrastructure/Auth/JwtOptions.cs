using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Infrastructure.Auth
{
    public record class JwtOptions
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public int ExpireHours { get; set; } = 1;
    }
}
