using LearnStore.Application.Commands.AuthCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators.AuthValidators
{
    public class AddRoleToUserCommandValidator : AbstractValidator<AddRoleToUserCommand>
    {
        public AddRoleToUserCommandValidator() 
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserIdIsRequired");
            RuleFor(x => x.Role).NotEmpty().WithMessage("RoleIsRequired");
        }
    }
}
