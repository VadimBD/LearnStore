
using FluentValidation;
using FluentValidation.Results;
using LearnStore.Application.Commands.AuthCommands;
using LearnStore.Application.Common;
using NSubstitute.ExceptionExtensions;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace LearnStore.Tests.Unit.Application.UseCases
{
    public class LoginHandlerTests
    {
        [Fact]
        public async Task Handle_WhenCommandIsNull_ShouldThrowArgumentNullException()
        {
            var authService = Substitute.For<IAuthService>();
            var validationRules = Substitute.For<IValidator<LoginCommand>>();
            var handler = new LoginHandler(authService, validationRules);
            
            var act = () => handler.Handle(null!, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }
        [Fact]
        public async Task Handle_WhenValidationFails_ShouldThrowValidationException()
        {
            var authService = Substitute.For<IAuthService>();
            var validationRules = Substitute.For<IValidator<LoginCommand>>();
            var validationFailures = new List<ValidationFailure>
            {
                new("Property1", "Error message 1")
            };
            validationRules.ValidateAsync(
                Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>())
                .ThrowsAsync(new FluentValidation.ValidationException(validationFailures));
            var handler = new LoginHandler(authService, validationRules);
            var command = new LoginCommand();
            
            var act = () => handler.Handle(command, CancellationToken.None);
            await act.Should().ThrowAsync<FluentValidation.ValidationException>();
        }
        [Fact]
        public async Task Handle_WhenCommandIsValid_ReturnsSuccessfulLoginResponse()
        {
            var authService = Substitute.For<IAuthService>();
            var validationRules = Substitute.For<IValidator<LoginCommand>>();
            validationRules.ValidateAsync(
                Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>())
                .Returns(new ValidationResult());
            authService.LoginAsync(
                Arg.Any<UserDto>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
                .Returns(new AuthResult(true, null, null, "123"));
            var handler = new LoginHandler(authService, validationRules);
            var command = new LoginCommand
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "password",
            };
            
            var result = await handler.Handle(command, CancellationToken.None);
            
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.UserId.Should().Be("123");
            result.Error.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task Handle_WhenLoginFails_ReturnsFailedLoginResponse()
        {
            var authService = Substitute.For<IAuthService>();
            var validationRules = Substitute.For<IValidator<LoginCommand>>();
            validationRules.ValidateAsync(
                Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>())
                .Returns(new ValidationResult());
            authService.LoginAsync(
                Arg.Any<UserDto>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
                .Returns(new AuthResult(false, "Invalid credentials."));
            var handler = new LoginHandler(authService, validationRules);
            var command = new LoginCommand
            {
                Name = "Test User",
                Email = "email"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Invalid credentials.");
        }
    }
}
