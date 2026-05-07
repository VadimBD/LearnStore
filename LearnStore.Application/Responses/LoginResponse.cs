using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Responses
{
    public class LoginResponse
    {
        public bool IsSuccess = false;
        public string? Error = string.Empty;
        public string? UserId = string.Empty;
        public List<string> Roles { get; set; } = [];
    }
}
