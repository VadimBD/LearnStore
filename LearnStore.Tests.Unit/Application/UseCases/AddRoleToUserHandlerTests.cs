using FluentValidation;
using FluentValidation.Results;
using LearnStore.Application.Commands.AuthCommands;
using NSubstitute.ExceptionExtensions;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace LearnStore.Tests.Unit.Application.UseCases
{
    public class AddRoleToUserHandlerTests
    {
        [Fact]
        public async Task Handle_WhenCommandIsNull_ShouldThrowArgumentNullException()
        {
            var authService = Substitute.For<IAuthService>();
            var validationRules = Substitute.For<IValidator<AddRoleToUserCommand>>();
            var handler = new AddRoleToUserHandler(authService, validationRules);

            var act = () => handler.Handle(null!, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }
        [Fact]
        public async Task Handle_WhenValidationFails_ShouldThrowValidationException()
        {
            var authService = Substitute.For<IAuthService>();
            var validationRules = Substitute.For<IValidator<AddRoleToUserCommand>>();
            var validationFailures = new List<ValidationFailure>
            {
                new("Property1", "Error message 1")
            };

            validationRules.ValidateAsync(
                Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>())
                .ThrowsAsync(new FluentValidation.ValidationException(validationFailures));
            var handler = new AddRoleToUserHandler(authService, validationRules);
            var command = new AddRoleToUserCommand();
            var act = () => handler.Handle(command, CancellationToken.None);
            await act.Should().ThrowAsync<FluentValidation.ValidationException>();
        }
        [Fact]
        public async Task Handle_WhenCommandIsValid_ShouldCallAuthServiceAddRoleToUserAsync()
        {
            var authService = Substitute.For<IAuthService>();
            var validationRules = Substitute.For<IValidator<AddRoleToUserCommand>>();
            validationRules.ValidateAsync(
                Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>())
                .Returns(new ValidationResult());
            var handler = new AddRoleToUserHandler(authService, validationRules);
            var command = new AddRoleToUserCommand { UserId = "UserId", Role = "Admin" };

            await handler.Handle(command, CancellationToken.None);
            await authService.Received(1).AddRoleToUserAsync(command.UserId, command.Role, Arg.Any<CancellationToken>());
        }

    }
}
