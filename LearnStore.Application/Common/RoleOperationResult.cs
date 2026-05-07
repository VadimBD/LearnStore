using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Common
{
    public class RoleOperationResult
    {
        public bool Success { get; init; }
        public string? ErrorMessage { get; init; }
        public string? RoleName { get; init; }
        public string? UserId { get; init; }
        public IEnumerable<string>? CurrentRoles { get; init; }
    }
}
