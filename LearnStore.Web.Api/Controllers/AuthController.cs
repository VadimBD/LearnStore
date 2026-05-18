
using LearnStore.Application.Commands.AuthCommands;

using LearnStore.Application.Queries;
using LearnStore.Web.Api.Models.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Features;

namespace LearnStore.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController (IMediator Mediator): ControllerBase
    {
        [HttpPost("token")]
        public async Task<IActionResult> GenerateToken([FromBody] TokenRequest request)
        {
            var loginCommand = new LoginCommand()
            {
                Email = request.Email,
                Name = request.Name,
                Password = request.Password,
            };
            var loginResult = await Mediator.Send(loginCommand);
            if (!loginResult.IsSuccess)
                return Unauthorized(new TokenResponse { IsSuccess = false, Error = loginResult.Error ?? string.Empty });
            var query = new GenerateTokenQuery
            {
                Id = loginResult.UserId ?? string.Empty,
                Name = request.Name,
                Email = request.Email,
                Roles = loginResult.Roles
            };
            var generateTokenResult = await Mediator.Send(query);
            var result = new TokenResponse
            {
                IsSuccess = !string.IsNullOrEmpty(generateTokenResult),
                Token = generateTokenResult,
            };
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request) 
        {
            var comand = new RegisterCommand()
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
            };
            var result = await Mediator.Send(comand);
            if (!result.IsSuccess)
                return BadRequest(new { Message = result.Error });
            return Ok(new RegisterResponse { IsSuccess = result.IsSuccess, UserId = result.UserId ?? string.Empty, Name = request.Name, Email = request.Email, Error = result.Error ?? string.Empty });
        }
        [HttpPost("AddRoleToUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoleToUser([FromBody] RoleOperationRequest request)
        {
            var command = new AddRoleToUserCommand
            {
                UserId = request.UserId,
                Role = request.Role
            };
            var result = await Mediator.Send(command);
            if (!result.Success)
                return BadRequest(new RoleOperationResponse { Success = false, ErrorMessage = result.ErrorMessage ?? string.Empty });
            return Ok(new RoleOperationResponse { Success = true, UserId = request.UserId, RoleName = request.Role, CurrentRoles = result.CurrentRoles ?? Array.Empty<string>() });
        }

        [HttpPost("RemoveRoleFromUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveRoleFromUser([FromBody] RoleOperationRequest request)
        {
            var command = new RemoveRoleFromUserCommand
            {
                UserId = request.UserId,
                Role = request.Role
            };
            var result = await Mediator.Send(command);
            if (!result.Success)
                return BadRequest(new RoleOperationResponse { Success = false, ErrorMessage = result.ErrorMessage ?? string.Empty });
            return Ok(new RoleOperationResponse { Success = true, UserId = request.UserId, RoleName = request.Role, CurrentRoles = result.CurrentRoles ?? Array.Empty<string>() });
        }
    }
}
