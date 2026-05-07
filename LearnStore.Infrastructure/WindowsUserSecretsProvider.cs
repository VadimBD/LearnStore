using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Infrastructure
{
    public class WindowsUserSecretsProvider : ISecretProvider
    {
        
      private readonly IConfiguration _configuration;
        public WindowsUserSecretsProvider(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public string GetSecret(string key)
        {
            var value = _configuration[key];
            if (string.IsNullOrEmpty(value))
                throw new InvalidOperationException($"Password for key '{key}' not found in configuration under 'Secrets:{key}'.");
            return value;
        }
    }
}
