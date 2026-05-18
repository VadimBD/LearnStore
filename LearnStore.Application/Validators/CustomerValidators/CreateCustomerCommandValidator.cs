using FluentValidation;
using LearnStore.Application.Commands.CustomerCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators.CustomerValidators
{
    public class CreateCustomerCommandValidator:AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(c => c!.Name).Cascade(CascadeMode.Stop).NotEmpty();
            RuleFor(c => c!.EmailAddress).Cascade(CascadeMode.Stop).NotEmpty().EmailAddress();
            RuleFor(c => c!.PhoneNumber).Cascade(CascadeMode.Stop).NotEmpty().Matches(@"^\+?[1-9]\d{1,14}$");
        }
    }
}
