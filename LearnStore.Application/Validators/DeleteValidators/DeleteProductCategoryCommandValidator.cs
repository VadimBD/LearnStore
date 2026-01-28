using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators
{
    public class DeleteProductCategoryCommandValidator : AbstractValidator<DeleteProductCategoryCommand>
    {
        public DeleteProductCategoryCommandValidator()
        {
            RuleFor(pc => pc.ProductId).GreaterThan(0);
        }
    }
}
