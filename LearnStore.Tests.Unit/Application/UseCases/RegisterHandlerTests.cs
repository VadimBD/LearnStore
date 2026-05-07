using FluentValidation;
using FluentValidation.Results;
using LearnStore.Application.Commands.AuthCommands;
using LearnStore.Application.Common;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using ValidationResult = FluentValidation.Results.ValidationResult;
namespace LearnStore.Tests.Unit.Application.UseCases
{
    public class RegisterHandlerTests
    {
        [Fact]
        public async Task Handle_WhenCommandIsNull_ShouldThrowArgumentNullException()
        {
            var authService = Substitute.For<IAuthService>();
            var validationRules = Substitute.For<IValidator<RegisterCommand>>();
            var handler = new RegisterHandler(authService, validationRules);
            
            var act = () => handler.Handle(null!, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Handle_WhenValidationFails_ShouldThrowValidationException()
        {
            var authService = Substitute.For<IAuthService>();
            var validationRules = Substitute.For<IValidator<RegisterCommand>>();
            var validationFailures = new List<ValidationFailure>
            {
                new("Property1", "Error message 1")
            };
            validationRules.ValidateAsync(
                Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>())
                .ThrowsAsync(new FluentValidation.ValidationException(validationFailures));
            var handler = new RegisterHandler(authService, validationRules);
            var command = new RegisterCommand();
            
            var act = () => handler.Handle(command, CancellationToken.None);
            await act.Should().ThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact]
        public async Task Handle_WhenCommandIsValid_ReturnsSuccessfulLoginResponse()
        {
            var authService = Substitute.For<IAuthService>();
            var validationRules = Substitute.For<IValidator<RegisterCommand>>();
            validationRules.ValidateAsync(
                Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>())
                .Returns(new ValidationResult());
            authService.RegisterAsync(
                Arg.Any<UserDto>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
                .Returns(new AuthResult { IsSuccess = true, UserId = "123" });
            var handler = new RegisterHandler(authService, validationRules);
            var command = new RegisterCommand
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
        public async Task Handle_WhenRegisterFails_ReturnsFailedRegisterResponse()
        {
            var authService = Substitute.For<IAuthService>();
            var validationRules = Substitute.For<IValidator<RegisterCommand>>();
            validationRules.ValidateAsync(
                Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>())
                .Returns(new ValidationResult());
            authService.RegisterAsync(
                Arg.Any<UserDto>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
                .Returns(new AuthResult { IsSuccess = false, Error = "Already exists." });
            var handler = new RegisterHandler(authService, validationRules);
            var command = new RegisterCommand
            {
                Name = "Test User",
                Email = "email"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Already exists.");
        }
    }
}
