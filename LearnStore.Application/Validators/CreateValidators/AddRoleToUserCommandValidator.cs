using LearnStore.Application.Commands.AddCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators.CreateValidators
{
    public class AddRoleToUserCommandValidator : AbstractValidator<AddRoleToUserCommand>
    {
        public AddRoleToUserCommandValidator() 
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
            RuleFor(x => x.Role).NotEmpty().WithMessage("Role is required.");
        }
    }
}
