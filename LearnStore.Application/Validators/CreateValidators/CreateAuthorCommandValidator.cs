using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators
{
    public class CreateAuthorCommandValidator:AbstractValidator<CreateAuthorCommand>
    {
        public CreateAuthorCommandValidator()
        {
            RuleFor(a => a.FirstName).Cascade(CascadeMode.Stop).NotEmpty();
            RuleFor(a => a.LastName).Cascade(CascadeMode.Stop).NotEmpty();
            RuleFor(a => a.MiddleName).Cascade(CascadeMode.Stop).NotEmpty();
            RuleFor(a => a.Info).Cascade(CascadeMode.Stop).NotEmpty();
        }

    }
}
