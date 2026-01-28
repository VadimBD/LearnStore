using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators
{
    public class CreatePaymentCommandValidator:AbstractValidator<CreatePaymentCommand>
    {
        public CreatePaymentCommandValidator()
        {
            RuleFor(p => p!.OrderId).NotEmpty();
            RuleFor(p => p!.Amount).GreaterThan(0);
        }
    }
}
