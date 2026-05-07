using LearnStore.Application.Commands.AuthCommands;
using LearnStore.Application.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class LoginHandler(IAuthService AuthService, IValidator<LoginCommand> ValidationRules) : IRequestHandler<LoginCommand, LoginResponse>
    {
        public async Task<LoginResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));
            await ValidationRules.ValidateAndThrowAsync(command, cancellationToken);
            var user = new UserDto()
            {
                Id=command.Id,
                Name=command.Name,
                Email=command.Email,
                Roles=command.Roles
            };
            var result= await AuthService.LoginAsync(user, command.Password, cancellationToken);
            return new() 
            {
                IsSuccess = result.IsSuccess,
                Error = result.Error,
                UserId = result.UserId,
                Roles = result.Roles
            };
        }
    }
}
