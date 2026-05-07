using LearnStore.Application.Commands.AuthCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators.AuthValidators
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().When(x => string.IsNullOrEmpty(x.Email)).WithMessage("Name is required.");
            RuleFor(x => x.Email).NotEmpty().When(x => string.IsNullOrEmpty(x.Name)).WithMessage("Email is required.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        }
    }
}
