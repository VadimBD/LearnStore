using LearnStore.Application.Commands.AddCommands;
using LearnStore.Application.Commands.DeleteCommands;
using LearnStore.Application.Validators.CreateValidators;
using LearnStore.Application.Validators.DeleteValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Validators
{
    public class AddRoleToUserCommandValidatorTests
    {
        [Fact]
        public void Validate_WhenCommandIsValid_ReturnsSuccess()
        {
            var validator = new AddRoleToUserCommandValidator();
            var command = new AddRoleToUserCommand
            {
                UserId = "UserId",
                Role = "Admin"
            };
            var result = validator.Validate(command);
            result.IsValid.Should().BeTrue();
        }
        [Fact]
        public void Validate_WhenUserIdIsEmpty_ReturnsValidationError()
        {
            var validator = new AddRoleToUserCommandValidator();
            var command = new AddRoleToUserCommand
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
                UserId = "UserId",
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
                UserId = "UserId",
                Role = null!
            };
            var result = validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Role" && e.ErrorMessage == "Role is required.");
        }
    }
}
