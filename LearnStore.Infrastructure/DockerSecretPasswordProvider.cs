using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Infrastructure
{
    public class DockerSecretPasswordProvider : IPasswordProvider
    {
        public string GetPassword(string key)
        {
            var path = $"/run/secrets/{key}";
            if (!File.Exists(path)) 
                throw new FileNotFoundException($"Docker secret file '{key}' not found at path '{path}'.");
            return File.ReadAllText(path).Trim();
        }
    }
}
