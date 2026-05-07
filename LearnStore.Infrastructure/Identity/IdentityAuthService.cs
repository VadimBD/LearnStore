
using LearnStore.Application.Common;
using LearnStore.Application.DTO;
using LearnStore.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LearnStore.Infrastructure.Identity
{
    public class IdentityAuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly RoleManager<IdentityRole> _roleManager;
        public IdentityAuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IJwtTokenGenerator jwtTokenGenerator, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _roleManager = roleManager;
        }
        public async Task<RoleOperationResult> AddRoleToUserAsync(string userId, string role, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(userId, nameof(userId));
            ArgumentException.ThrowIfNullOrEmpty(role, nameof(role));
            var identityUser = await _userManager.FindByIdAsync(userId);
            if (identityUser == null) 
                return new RoleOperationResult() { Success=false, ErrorMessage = "User not found."};
            if (await _roleManager.FindByNameAsync(role) is null)
                return new RoleOperationResult { Success = false, ErrorMessage = "Role not found." };
            if (await _userManager.IsInRoleAsync(identityUser, role))
                return new RoleOperationResult { Success = false, ErrorMessage = "User already has the role." };
            await _userManager.AddToRoleAsync(identityUser, role);
            return new RoleOperationResult { Success = true, RoleName = role, UserId = userId, CurrentRoles = await _userManager.GetRolesAsync(identityUser) };
        }
        public Task<string> GenerateJwtTokenAsync(UserDto user, CancellationToken cancellationToken)
        {
            return Task.FromResult(_jwtTokenGenerator.GenerateToken(user));
        }
        public async Task<AuthResult> LoginAsync(UserDto user, string password, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            if (string.IsNullOrEmpty(user.Name) && string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(password))
                throw new ArgumentException("Username or email and password must be provided.");

            var identityUser = await _userManager.FindByNameAsync(user.Name) ?? await _userManager.FindByEmailAsync(user.Email);
            if (identityUser is null)
                return new AuthResult(false, "Invalid username or email.");

            await _signInManager.SignOutAsync();
            var signInResult = await _signInManager.PasswordSignInAsync(identityUser, password, false, false);

            return signInResult.Succeeded ? new AuthResult(true, null, null, identityUser.Id) : new AuthResult(false, "Invalid password.");
        }
        public async Task LogoutAsync(CancellationToken cancellationToken) => await _signInManager.SignOutAsync();
        public async Task<AuthResult> RegisterAsync(UserDto user, string password, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            if (string.IsNullOrEmpty(user.Name) && string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(password))
                throw new ArgumentException("Username or email and password must be provided.");
            var identityUser = new IdentityUser() { UserName = user.Name, Email = user.Email };
            var result = await _userManager.CreateAsync(identityUser, password);
            if (!result.Succeeded)
                return new AuthResult(false, string.Join("; ", result.Errors.Select(e => e.Description)));
            return new AuthResult(result.Succeeded, result.Succeeded ? null : "User registration failed.", null, identityUser.Id);
        }

        public async Task<RoleOperationResult> RemoveRoleFromUserAsync(string userId, string role, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(userId, nameof(userId));
            ArgumentException.ThrowIfNullOrEmpty(role, nameof(role));
            var identyUser = await _userManager.FindByIdAsync(userId);
            if (identyUser == null)
                return new RoleOperationResult { Success = false, ErrorMessage = "User not found." };
            if (await _roleManager.FindByNameAsync(role) is null)
                return new RoleOperationResult { Success = false, ErrorMessage = "Role not found." };
            if (!await _userManager.IsInRoleAsync(identyUser, role))
                return new RoleOperationResult { Success = false, ErrorMessage = "User does not have the role." };
            await _userManager.RemoveFromRoleAsync(identyUser, role);
            return new RoleOperationResult { Success = true, RoleName = role, UserId = userId, CurrentRoles = await _userManager.GetRolesAsync(identyUser) };
        }
    }
}
