using LearnStore.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(UserDto user, string password, CancellationToken cancellationToken);
        Task<AuthResult> LoginAsync(UserDto user, string password, CancellationToken cancellationToken);
        Task LogoutAsync(CancellationToken cancellationToken);
        Task<string> GenerateJwtTokenAsync(UserDto user, CancellationToken cancellationToken);
        Task<RoleOperationResult> AddRoleToUserAsync(string userId, string role, CancellationToken cancellationToken);
        Task<RoleOperationResult> RemoveRoleFromUserAsync(string userId, string role, CancellationToken cancellationToken);
    }
}
