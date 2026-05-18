using FluentValidation;
using LearnStore.Application.Commands.SellerCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators.SellerValidators
{
    public class CreateSellerCommandValidator:AbstractValidator<CreateSellerCommand>
    {
        public CreateSellerCommandValidator()
        {
            RuleFor(s => s!.Name).Cascade(CascadeMode.Stop).NotEmpty();
            RuleFor(s => s!.EmailAddress).Cascade(CascadeMode.Stop).NotEmpty().EmailAddress();
            RuleFor(s => s!.PhoneNumber).Cascade(CascadeMode.Stop).NotEmpty().Matches(@"^\+?[1-9]\d{1,14}$");
        }
    }
}
