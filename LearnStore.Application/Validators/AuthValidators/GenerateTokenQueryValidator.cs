using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators.AuthValidators
{
    public class GenerateTokenQueryValidator : AbstractValidator<GenerateTokenQuery>
    {
        public GenerateTokenQueryValidator()
        {
            RuleFor(x => x).Must(x => !string.IsNullOrEmpty(x.Name) || !string.IsNullOrEmpty(x.Email)).WithMessage("Either Name or Email must be provided.");
        }
    }
}
