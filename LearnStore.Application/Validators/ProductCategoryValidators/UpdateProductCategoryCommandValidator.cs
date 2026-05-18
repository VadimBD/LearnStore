using LearnStore.Application.Commands.ProductCategoryCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators.ProductCategoryValidators
{
    public class UpdateProductCategoryCommandValidator:AbstractValidator<UpdateProductCategoryCommand>
    {
        public UpdateProductCategoryCommandValidator()
        {
            RuleFor(c => c!.Id).GreaterThan(0);
            RuleFor(c => c!.Name).Cascade(CascadeMode.Stop).NotEmpty();
        }
    }
}
