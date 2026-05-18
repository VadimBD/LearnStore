using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class GenerateTokenHandler(IAuthService AuthService, IValidator<GenerateTokenQuery> ValidationRules): IRequestHandler<GenerateTokenQuery, string>
    {
        public async Task<string> Handle(GenerateTokenQuery query, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));
            await ValidationRules.ValidateAndThrowAsync(query, cancellationToken: cancellationToken);
            var user = new UserDto
            {
                Id = query.Id,
                Name = query.Name,
                Email = query.Email,
                Roles = query.Roles,

            };
            return await AuthService.GenerateJwtTokenAsync(user, cancellationToken);
        }
    }
}
