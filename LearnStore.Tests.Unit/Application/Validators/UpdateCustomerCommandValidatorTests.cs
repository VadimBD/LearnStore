using LearnStore.Application.Commands.CustomerCommands;
using LearnStore.Application.Validators.CustomerValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Validators
{
    public class UpdateCustomerCommandValidatorTests
    {
        private readonly Fixture _fixture;
        private readonly Faker _faker = new();
        public UpdateCustomerCommandValidatorTests()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new RandomNumericSequenceGenerator(1, 100));
        }
        [Fact]
        public void Validate_WhenIdIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateCustomerCommandValidator();
            var command = new UpdateCustomerCommand()
            {
                Id = string.Empty,
                Name = _faker.Name.FullName(),
                EmailAddress = _faker.Internet.Email(),
                PhoneNumber = "+380" + _faker.Random.ReplaceNumbers("#########")
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
            var validator = new UpdateCustomerCommandValidator();
            var command = new UpdateCustomerCommand()
            {
                Id = "1",
                Name = _faker.Name.FullName(),
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
        public void Validate_WhenEmailAddressIsInvalid_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateCustomerCommandValidator();
            var command = new UpdateCustomerCommand()
            {
                Id = "1",
                Name = _faker.Name.FullName(),
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
            var validator = new UpdateCustomerCommandValidator();
            var command = new UpdateCustomerCommand()
            {
                Id = "1",
                Name = _faker.Name.FullName(),
                EmailAddress = _faker.Internet.Email(),
                PhoneNumber = "invalid-phone"
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "PhoneNumber");
        }
        [Fact]
        public void Validate_WhenNameIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateCustomerCommandValidator();
            var command = new UpdateCustomerCommand()
            {
                Id = "1",
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
        public void Validate_WhenPhoneNumberIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateCustomerCommandValidator();
            var command = new UpdateCustomerCommand()
            {
                Id = "1",
                Name = _faker.Name.FullName(),
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
        public void Validate_WhenEmailAddressIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateCustomerCommandValidator();
            var command = new UpdateCustomerCommand()
            {
                Id = "1",
                Name = _faker.Name.FullName(),
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
        public void Validate_WhenNameIsWhitespace_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateCustomerCommandValidator();
            var command = new UpdateCustomerCommand()
            {
                Id = "1",
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
        public void Validate_WhenPhoneNumberIsWhitespace_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateCustomerCommandValidator();
            var command = new UpdateCustomerCommand()
            {
                Id = "1",
                Name = _faker.Name.FullName(),
                EmailAddress = _faker.Internet.Email(),
                PhoneNumber = "   "
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "PhoneNumber");
        }
        [Fact]
        public void Validate_WhenEmailAddressIsWhitespace_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateCustomerCommandValidator();
            var command = new UpdateCustomerCommand()
            {
                Id = "1",
                Name = _faker.Name.FullName(),
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
        public void Validate_WhenPhoneNumberIsTooLong_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateCustomerCommandValidator();
            var command = new UpdateCustomerCommand()
            {
                Id = "1",
                Name = _faker.Name.FullName(),
                EmailAddress = _faker.Internet.Email(),
                PhoneNumber = "+12345678901234567" // 17 digits, exceeding E.164 limit
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
            var validator = new UpdateCustomerCommandValidator();
            var command = new UpdateCustomerCommand()
            {
                Id = "1",
                Name = _faker.Name.FullName(),
                EmailAddress = _faker.Internet.Email(),
                PhoneNumber = "+1" // Too short to be valid
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "PhoneNumber");
        }
        [Fact]
        public void Validate_WhenEmailAddressLacksAtSymbol_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateCustomerCommandValidator();
            var command = new UpdateCustomerCommand()
            {
                Id = "1",
                Name = _faker.Name.FullName(),
                EmailAddress = "userdomain.com", // Missing '@' symbol
                PhoneNumber = "+380" + _faker.Random.ReplaceNumbers("#########")
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "EmailAddress");
        }
        [Fact]
        public void Validate_WhenEmailAddressLacksDomain_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateCustomerCommandValidator();
            var command = new UpdateCustomerCommand()
            {
                Id = "1",
                Name = _faker.Name.FullName(),
                EmailAddress = "user@", // Missing domain part
                PhoneNumber = "+380" + _faker.Random.ReplaceNumbers("#########")
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "EmailAddress");
        }
        [Fact]
        public void Validate_WhenEmailAddressLacksUsername_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateCustomerCommandValidator();
            var command = new UpdateCustomerCommand()
            {
                Id = "1",
                Name = _faker.Name.FullName(),
                EmailAddress = "@domain.com", // Missing username part
                PhoneNumber = "+380" + _faker.Random.ReplaceNumbers("#########")
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "EmailAddress");
        }

    }
}
