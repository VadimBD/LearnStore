using LearnStore.Application.Commands.CustomerCommands;
using LearnStore.Application.Validators.CustomerValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Validators
{
    public class CreateCustomerCommandValidatorTests
    {
        private readonly Fixture _fixture;
        private readonly Faker _faker = new();
        public CreateCustomerCommandValidatorTests()
        {
            _fixture = new Fixture();
            _faker = new Faker();
        }

        [Fact]
        public void Validate_WhenAllPropertiesAreValid_Success()
        {
            // Arrange
            var validator = new CreateCustomerCommandValidator();
            var command = new CreateCustomerCommand()
            {
                Name = _faker.Name.FirstName(),
                EmailAddress = _faker.Internet.Email(),
                PhoneNumber = "+380" + _faker.Random.ReplaceNumbers("#########")
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
        [Fact]
        public void Validate_WhenNameIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateCustomerCommandValidator();
            var command = new CreateCustomerCommand()
            {
                Name = string.Empty,
                EmailAddress = _faker.Internet.Email(),
                PhoneNumber = "+380" + _faker.Random.ReplaceNumbers("#########")
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Name");
        }
        [Fact]
        public void Validate_WhenEmailAddressIsInvalid_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateCustomerCommandValidator();
            var command = new CreateCustomerCommand()
            {
                Name = _faker.Name.FirstName(),
                EmailAddress = "invalid-email",
                PhoneNumber = "+380" + _faker.Random.ReplaceNumbers("#########")
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "EmailAddress");
        }
        [Fact]
        public void Validate_WhenPhoneNumberIsInvalid_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateCustomerCommandValidator();
            var command = new CreateCustomerCommand()
            {
                Name = _faker.Name.FirstName(),
                EmailAddress = _faker.Internet.Email(),
                PhoneNumber = "012345"
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "PhoneNumber");
        }
        [Fact]
        public void Validate_WhenMultiplePropertiesAreInvalid_ReturnsMultipleValidationErrors()
        {
            // Arrange
            var validator = new CreateCustomerCommandValidator();
            var command = new CreateCustomerCommand()
            {
                Name = string.Empty,
                EmailAddress = "invalid-email",
                PhoneNumber = "012345"
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(3);
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
            result.Errors.Should().Contain(e => e.PropertyName == "EmailAddress");
            result.Errors.Should().Contain(e => e.PropertyName == "PhoneNumber");
        }
        [Fact]
        public void Validate_WhenEmailAddressIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateCustomerCommandValidator();
            var command = new CreateCustomerCommand()
            {
                Name = _faker.Name.FirstName(),
                EmailAddress = string.Empty,
                PhoneNumber = "+380" + _faker.Random.ReplaceNumbers("#########")
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "EmailAddress");
        }
        [Fact]
        public void Validate_WhenPhoneNumberIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateCustomerCommandValidator();
            var command = new CreateCustomerCommand()
            {
                Name = _faker.Name.FirstName(),
                EmailAddress = _faker.Internet.Email(),
                PhoneNumber = string.Empty
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "PhoneNumber");
        }
        [Fact]
        public void Validate_WhenNameIsWhitespace_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateCustomerCommandValidator();
            var command = new CreateCustomerCommand()
            {
                Name = "   ",
                EmailAddress = _faker.Internet.Email(),
                PhoneNumber = "+380" + _faker.Random.ReplaceNumbers("#########")
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Name");

        }
        [Fact]
        public void Validate_WhenPhoneNumberHasInvalidFormat_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateCustomerCommandValidator();
            var command = new CreateCustomerCommand()
            {
                Name = _faker.Name.FirstName(),
                EmailAddress = _faker.Internet.Email(),
                PhoneNumber = "abcdefg"
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "PhoneNumber");
        }
        [Fact]
        public void Validate_WhenEmailAddressHasWhitespace_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateCustomerCommandValidator();
            var command = new CreateCustomerCommand()
            {
                Name = _faker.Name.FirstName(),
                EmailAddress = "   ",
                PhoneNumber = "+380" + _faker.Random.ReplaceNumbers("#########")
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "EmailAddress");
        }
        [Fact]
        public void Validate_WhenNameIsNull_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateCustomerCommandValidator();
            var command = new CreateCustomerCommand()
            {
                Name = null,
                EmailAddress = _faker.Internet.Email(),
                PhoneNumber = "+380" + _faker.Random.ReplaceNumbers("#########")
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Name");
        }
        [Fact]
        public void Validate_WhenEmailAddressIsNull_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateCustomerCommandValidator();
            var command = new CreateCustomerCommand()
            {
                Name = _faker.Name.FirstName(),
                EmailAddress = null,
                PhoneNumber = "+380" + _faker.Random.ReplaceNumbers("#########")
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "EmailAddress");
        }
        [Fact]
        public void Validate_WhenPhoneNumberIsNull_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateCustomerCommandValidator();
            var command = new CreateCustomerCommand()
            {
                Name = _faker.Name.FirstName(),
                EmailAddress = _faker.Internet.Email(),
                PhoneNumber = null
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "PhoneNumber");
        }
        [Fact]
        public void Validate_WhenPhoneNumberIsTooShort_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateCustomerCommandValidator();
            var command = new CreateCustomerCommand()
            {
                Name = _faker.Name.FirstName(),
                EmailAddress = _faker.Internet.Email(),
                PhoneNumber = "+1"
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "PhoneNumber");
        }
        [Fact]
        public void Validate_WhenPhoneNumberIsTooLong_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateCustomerCommandValidator();
            var command = new CreateCustomerCommand()
            {
                Name = _faker.Name.FirstName(),
                EmailAddress = _faker.Internet.Email(),
                PhoneNumber = "+12345678901234567"
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "PhoneNumber");
        }

    }
}
