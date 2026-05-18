using LearnStore.Application.Commands.AuthorCommands;
using LearnStore.Application.Validators.AuthorValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Validators
{
    public class DeleteAuthorCommandValidatorTests
    {
        private readonly Fixture _fixture;
        private readonly Faker _faker = new();
        public DeleteAuthorCommandValidatorTests()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new RandomNumericSequenceGenerator(1, 100));
        }
        [Fact]
        public void Validate_WhenIdIsInvalid_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new DeleteAuthorCommandValidator();
            var command = new DeleteAuthorCommand(0);
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "AuthorId");
        }
        [Fact]
        public void Validate_WhenIdIsValid_Success()
        {
            // Arrange
            var validator = new DeleteAuthorCommandValidator();
            var command = new DeleteAuthorCommand(10);
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
