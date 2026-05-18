using LearnStore.Application.Commands.AuthCommands;

namespace LearnStore.Application.Validators.AuthValidators
{
    public class RemoveRoleFromUserCommandValidator : AbstractValidator<RemoveRoleFromUserCommand>
    {
        public RemoveRoleFromUserCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
            RuleFor(x => x.Role).NotEmpty().WithMessage("Role is required.");
        }
    }
}
