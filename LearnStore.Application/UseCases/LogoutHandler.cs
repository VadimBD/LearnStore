using LearnStore.Application.Commands.AuthCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class LogoutHandler(IAuthService AuthService): IRequestHandler<LogoutCommand, Unit>
    {
        public async Task<Unit> Handle(LogoutCommand command, CancellationToken cancellationToken)
        {
            await AuthService.LogoutAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
