using LearnStore.Application.Commands.AuthCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators.AuthValidators
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x).Must(x => !string.IsNullOrEmpty(x.Name) || !string.IsNullOrEmpty(x.Email)).WithMessage("Either Name or Email must be provided.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password must be provided.");
        }
    }
}
