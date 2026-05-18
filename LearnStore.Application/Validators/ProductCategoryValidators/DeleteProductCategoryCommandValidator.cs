using LearnStore.Application.Commands.ProductCategoryCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators.ProductCategoryValidators
{
    public class DeleteProductCategoryCommandValidator : AbstractValidator<DeleteProductCategoryCommand>
    {
        public DeleteProductCategoryCommandValidator()
        {
            RuleFor(pc => pc.ProductId).GreaterThan(0);
        }
    }
}
