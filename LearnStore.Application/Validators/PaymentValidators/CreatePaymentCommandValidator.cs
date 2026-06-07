using FluentValidation;
using LearnStore.Application.Commands.PaymentCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators.PaymentValidators
{
    public class CreatePaymentCommandValidator:AbstractValidator<CreatePaymentCommand>
    {
        public CreatePaymentCommandValidator()
        {
            RuleFor(p => p!.OrderId).NotEmpty();
            RuleFor(p => p!.Status).IsInEnum();
            RuleFor(p => p!.TransactionId).NotEmpty();
        }
    }
}
