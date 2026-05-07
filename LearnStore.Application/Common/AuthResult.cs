using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Common
{
    public record class AuthResult
    {
        public bool IsSuccess { get; init; }
        public string? Error { get; init; }
        public string? Token { get; init; }
        public string? UserId { get; init; }
        public List<string> Roles { get; init; } = [];
    }
}