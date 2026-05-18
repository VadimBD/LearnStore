using LearnStore.Application.Commands.AuthorCommands;
using LearnStore.Application.Validators.AuthorValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Validators
{
    public class UpdateAuthorCommandValidatorTests
    {
        private readonly Fixture _fixture;
        private readonly Faker _faker = new();
        public UpdateAuthorCommandValidatorTests()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new RandomNumericSequenceGenerator(1, 100));
        }
        [Fact]
        public void Validate_WhenIdIsLessThanOrEqualToZero_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateAuthorCommandValidator();
            var command = new UpdateAuthorCommand()
            {
                Id = 0,
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                MiddleName = _faker.Name.FirstName(),
                Info = "Test"
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Id");
        }
        [Fact]
        public void Validate_WhenAllPropertiesAreValid_Success()
        {
            // Arrange
            var validator = new UpdateAuthorCommandValidator();
            var command = new UpdateAuthorCommand()
            {
                Id = _fixture.Create<int>(),
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
            var validator = new UpdateAuthorCommandValidator();
            var command = new UpdateAuthorCommand()
            {
                Id = _fixture.Create<int>(),
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
        public void Validate_WhenFirstNameIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateAuthorCommandValidator();
            var command = new UpdateAuthorCommand()
            {
                Id = _fixture.Create<int>(),
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
        public void Validate_WhenMiddleNameIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateAuthorCommandValidator();
            var command = new UpdateAuthorCommand()
            {
                Id = _fixture.Create<int>(),
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
            var validator = new UpdateAuthorCommandValidator();
            var command = new UpdateAuthorCommand()
            {
                Id = _fixture.Create<int>(),
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
        public void Validate_WhenMultiplePropertiesAreInvalid_ReturnsMultipleValidationErrors()
        {
            // Arrange
            var validator = new UpdateAuthorCommandValidator();
            var command = new UpdateAuthorCommand()
            {
                Id = -1,
                FirstName = string.Empty,
                LastName = string.Empty,
                MiddleName = string.Empty,
                Info = string.Empty
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(5);
            result.Errors.Should().Contain(e => e.PropertyName == "Id");
            result.Errors.Should().Contain(e => e.PropertyName == "FirstName");
            result.Errors.Should().Contain(e => e.PropertyName == "LastName");
            result.Errors.Should().Contain(e => e.PropertyName == "MiddleName");
            result.Errors.Should().Contain(e => e.PropertyName == "Info");
        }
        [Fact]
        public void Validate_WhenFirstNameIsWhitespace_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateAuthorCommandValidator();
            var command = new UpdateAuthorCommand()
            {
                Id = _fixture.Create<int>(),
                FirstName = "   ",
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
        public void Validate_WhenLastNameIsWhitespace_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateAuthorCommandValidator();
            var command = new UpdateAuthorCommand()
            {
                Id = _fixture.Create<int>(),
                FirstName = _faker.Name.FirstName(),
                LastName = "   ",
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
        public void Validate_WhenMiddleNameIsWhitespace_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateAuthorCommandValidator();
            var command = new UpdateAuthorCommand()
            {
                Id = _fixture.Create<int>(),
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                MiddleName = "   ",
                Info = "Test"
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "MiddleName");
        }
        [Fact]
        public void Validate_WhenInfoIsWhitespace_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateAuthorCommandValidator();
            var command = new UpdateAuthorCommand()
            {
                Id = _fixture.Create<int>(),
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                MiddleName = _faker.Name.FirstName(),
                Info = "   "
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Info");
        }
    }
}
