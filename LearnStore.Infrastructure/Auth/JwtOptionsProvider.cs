using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Infrastructure.Auth
{
    public class JwtOptionsProvider(IConfiguration Configuration, ISecretProvider SecretProvider) : IJwtOptionsProvider
    {
        private const int DefaultJwtExpireHours = 1;

        public JwtOptions GetOptions() => new()
        {
            Key = GetJwtKey(),
            Issuer = Configuration["Jwt:Issuer"] ?? "LearnStore",
            Audience = Configuration["Jwt:Audience"] ?? "LearnStoreUser",
            ExpireHours = int.TryParse(Configuration["Jwt:ExpireHours"], out var hours) ? hours : DefaultJwtExpireHours
        };

        private string GetJwtKey() 
        {
            var jwtKey = Configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            { 
                var secretName= Configuration["Jwt:KeySecretName"];
                if (string.IsNullOrEmpty(secretName))
                {
                    throw new InvalidOperationException("JWT key is not configured.");
                }
                jwtKey = SecretProvider.GetSecret(secretName);
            }

            return jwtKey;
        }
    }
}