using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Common
{
    public record class AuthResult
    {
        public AuthResult(bool isSuccess, string? error = null, string? token = null, string? userId = null)
        {
            IsSuccess = isSuccess;
            Error = error;
            Token = token;
            UserId = userId;
        }

        public bool IsSuccess { get; }
        public string? Error { get; }
        public string? Token { get; }
        public string? UserId { get; }
    }
}
