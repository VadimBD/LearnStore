using FluentValidation;
using LearnStore.Application.Commands.ProductCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators.ProductValidators
{
    public class CreateProductCommandValidator:AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(p => p!.Name).Cascade(CascadeMode.Stop).NotEmpty();
            RuleFor(p => p!.Description).NotEmpty();
            RuleFor(p => p!.Price).GreaterThan(0);
            RuleFor(p => p!.IsActive).NotNull();
        }
    }
}
