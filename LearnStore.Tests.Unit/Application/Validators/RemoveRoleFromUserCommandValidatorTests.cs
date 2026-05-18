
using LearnStore.Application.Commands.AuthCommands;
using LearnStore.Application.Validators.AuthValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Validators
{
    public class RemoveRoleFromUserCommandValidatorTests
    {
        [Fact]
        public void Validate_WhenCommandIsValid_ReturnsSuccess()
        {
            var validator = new RemoveRoleFromUserCommandValidator();
            var command = new RemoveRoleFromUserCommand
            {
                UserId = "UserID",
                Role = "Admin"
            };
            var result = validator.Validate(command);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_WhenUserIdIsEmpty_ReturnsValidationError()
        {
            var validator = new RemoveRoleFromUserCommandValidator();
            var command = new RemoveRoleFromUserCommand
            {
                UserId = string.Empty,
                Role = "Admin"
            };
            var result = validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "UserId" && e.ErrorMessage == "UserId is required.");
        }

        [Fact]
        public void Validate_WhenRoleIsEmpty_ReturnsValidationError()
        {
            var validator = new RemoveRoleFromUserCommandValidator();
            var command = new RemoveRoleFromUserCommand
            {
                UserId = "UserID",
                Role = string.Empty
            };
            var result = validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Role" && e.ErrorMessage == "Role is required.");
        }

        [Fact]
        public void Validate_WhenUserIsNull_ReturnsValidationError()
        {
            var validator = new RemoveRoleFromUserCommandValidator();
            var command = new RemoveRoleFromUserCommand
            {
                UserId = null!,
                Role = "Admin"
            };
            var result = validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "UserId" && e.ErrorMessage == "UserId is required.");

        }

        [Fact]
        public void Validate_WhenRoleIsNull_ReturnsValidationError()
        {
            var validator = new RemoveRoleFromUserCommandValidator();
            var command = new RemoveRoleFromUserCommand
            {
                UserId = "UserID",
                Role = null!
            };
            var result = validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Role" && e.ErrorMessage == "Role is required.");
        }
    }
}
