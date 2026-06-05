using LearnStore.Application.Commands.AuthCommands;

namespace LearnStore.Application.Validators.AuthValidators
{
    public class RemoveRoleFromUserCommandValidator : AbstractValidator<RemoveRoleFromUserCommand>
    {
        public RemoveRoleFromUserCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserIdIsRequired");
            RuleFor(x => x.Role).NotEmpty().WithMessage("RoleIsRequired");
        }
    }
}
