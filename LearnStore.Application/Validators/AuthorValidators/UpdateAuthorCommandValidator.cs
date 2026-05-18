using FluentValidation;
using LearnStore.Application.Commands.AuthorCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators.AuthorValidators
{
    public class UpdateAuthorCommandValidator:AbstractValidator<UpdateAuthorCommand>
    {
        public UpdateAuthorCommandValidator()
        {
            RuleFor(a => a!.Id).GreaterThan(0);
            RuleFor(a => a!.FirstName).NotEmpty();
            RuleFor(a => a!.LastName).NotEmpty();
            RuleFor(a => a!.MiddleName).NotEmpty();
            RuleFor(a => a!.Info).NotEmpty();
        }
    }
}
