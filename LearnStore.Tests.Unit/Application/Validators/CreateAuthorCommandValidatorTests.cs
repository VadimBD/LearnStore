using LearnStore.Application.Commands;
using LearnStore.Application.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Validators
{
    public class CreateAuthorCommandValidatorTests
    {
        private readonly Fixture _fixture;
        private readonly Faker _faker = new();

        public CreateAuthorCommandValidatorTests()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new RandomNumericSequenceGenerator(1, 100));
        }
        [Fact]
        public void Validate_WhenFirstNameIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateAuthorCommandValidator();
            var command = new CreateAuthorCommand()
            {
                FirstName = string.Empty,
                LastName = _faker.Name.LastName(),
                MiddleName = _faker.Name.FirstName(),
                Info = "Test"
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "FirstName");
        }
        [Fact]
        public void Validate_WhenAllPropertiesAreValid_Success()
        {
            // Arrange
            var validator = new CreateAuthorCommandValidator();
            var command = new CreateAuthorCommand()
            {
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                MiddleName = _faker.Name.FirstName(),
                Info = "Test"
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
        [Fact]
        public void Validate_WhenLastNameIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateAuthorCommandValidator();
            var command = new CreateAuthorCommand()
            {
                FirstName = _faker.Name.FirstName(),
                LastName = string.Empty,
                MiddleName = _faker.Name.FirstName(),
                Info = "Test"
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "LastName");
        }
        [Fact]
        public void Validate_WhenMiddleNameIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateAuthorCommandValidator();
            var command = new CreateAuthorCommand()
            {
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                MiddleName = string.Empty,
                Info = "Test"
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "MiddleName");
        }

        [Fact]
        public void Validate_WhenInfoIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateAuthorCommandValidator();
            var command = new CreateAuthorCommand()
            {
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                MiddleName = _faker.Name.FirstName(),
                Info = string.Empty
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Info");
        }
        [Fact]
        public void Validate_WhenMultiplePropertiesAreEmpty_ReturnsMultipleValidationErrors()
        {
            // Arrange
            var validator = new CreateAuthorCommandValidator();
            var command = new CreateAuthorCommand()
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                MiddleName = _faker.Name.FirstName(),
                Info = string.Empty
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(3);
            result.Errors.Should().Contain(e => e.PropertyName == "FirstName");
            result.Errors.Should().Contain(e => e.PropertyName == "LastName");
            result.Errors.Should().Contain(e => e.PropertyName == "Info");
        }
        [Fact]
        public void Validate_WhenAllPropertiesAreEmpty_ReturnsValidationErrorsForAllProperties()
        {
            // Arrange
            var validator = new CreateAuthorCommandValidator();
            var command = new CreateAuthorCommand()
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                MiddleName = string.Empty,
                Info = string.Empty
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(4);
            result.Errors.Should().Contain(e => e.PropertyName == "FirstName");
            result.Errors.Should().Contain(e => e.PropertyName == "LastName");
            result.Errors.Should().Contain(e => e.PropertyName == "MiddleName");
            result.Errors.Should().Contain(e => e.PropertyName == "Info");
        }
    }
}
