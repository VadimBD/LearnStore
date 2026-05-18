using FluentValidation;
using LearnStore.Application.Commands.ProductCategoryCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators.ProductCategoryValidators
{
    public class CreateProductCategoryCommandValidator : AbstractValidator<CreateProductCategoryCommand>
    {
        public CreateProductCategoryCommandValidator()
        {
            RuleFor(pc => pc!.Name).Cascade(CascadeMode.Stop).NotEmpty();
        }
    }
}
