using LearnStore.Application.Commands.AuthCommands;
using LearnStore.Application.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class RegisterHandler(IAuthService AuthService, IValidator<RegisterCommand> ValidationRules) : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            await ValidationRules.ValidateAndThrowAsync(request, cancellationToken);

            var result = await AuthService.RegisterAsync(new UserDto { Name = request.Name, Email = request.Email }, request.Password, cancellationToken);
            foreach (var role in request.Roles)
                await AuthService.AddRoleToUserAsync(result.UserId, role, cancellationToken);
            return new()
            {
                IsSuccess = result.IsSuccess,
                Error = result.Error,
                UserId = result.UserId,
            };
        }
    }
}
