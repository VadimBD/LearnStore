using LearnStore.Application.Commands.AddCommands;
using LearnStore.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class AddRoleToUserHandler(IAuthService AuthService, IValidator<AddRoleToUserCommand> ValidationRules) : IRequestHandler<AddRoleToUserCommand, RoleOperationResult>
    {
        public async Task<RoleOperationResult> Handle(AddRoleToUserCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            await ValidationRules.ValidateAndThrowAsync(request, cancellationToken);
            return await AuthService.AddRoleToUserAsync(request.UserId, request.Role, cancellationToken);
        }
    }
}
