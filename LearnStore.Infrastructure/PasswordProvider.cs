using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Infrastructure
{
    public class PasswordProvider:IPasswordProvider
    {
        private readonly ISecretProvider _secretProvider;
        public PasswordProvider(ISecretProvider secretProvider)
        {
            _secretProvider = secretProvider;
        }
        public string GetPassword(string key)=>_secretProvider.GetSecret(key);
    }
}
