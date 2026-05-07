using LearnStore.Application.Commands.AuthCommands;
using LearnStore.Application.Validators.AuthValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Validators
{
    public class LoginCommandValidatorTests
    {
        [Fact]
        public void Validate_WhenCommandIsValid_ReturnsSuccess()
        {
            var commnd = new LoginCommand
            {
                Name = "Name",
                Email = "Name",
                Password = "Password"
            };
            var validator = new LoginCommandValidator();
            var result = validator.Validate(commnd);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_WhenNameAndEmailAreEmpty_ReturnsValidationError()
        {
            var commnd = new LoginCommand
            {
                Name = string.Empty,
                Email = string.Empty,
                Password = "Password"
            };
            var validator = new LoginCommandValidator();
            var result = validator.Validate(commnd);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == "Either Name or Email must be provided.");
        }

        [Fact]
        public void Validate_WhenPasswordIsEmpty_ReturnsValidationError()
        {
            var commnd = new LoginCommand
            {
                Name = "Name",
                Email = "Name",
                Password = string.Empty
            };
            var validator = new LoginCommandValidator();
            var result = validator.Validate(commnd);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == "Password must be provided.");
        }
    }
}
