using LearnStore.Application.Commands.AuthCommands;
using LearnStore.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class RemoveRoleFromUserHandler(IAuthService AuthService, IValidator<RemoveRoleFromUserCommand> ValidationRules) : IRequestHandler<RemoveRoleFromUserCommand, RoleOperationResult>
    {
        public async Task<RoleOperationResult> Handle(RemoveRoleFromUserCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            await ValidationRules.ValidateAndThrowAsync(request, cancellationToken);
            return await AuthService.RemoveRoleFromUserAsync(request.UserId, request.Role, cancellationToken);
        }
    }
}
