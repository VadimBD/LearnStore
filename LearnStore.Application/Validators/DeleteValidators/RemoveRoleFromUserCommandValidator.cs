using LearnStore.Application.Commands.DeleteCommands;


namespace LearnStore.Application.Validators.DeleteValidators
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
