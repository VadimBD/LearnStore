using LearnStore.Application.Commands.SellerCommands;
using LearnStore.Application.Validators.SellerValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Validators
{
    public class CreateSellerCommandValidatorTests
    {
        private readonly Fixture _fixture;
        private readonly Faker _faker = new();
        public CreateSellerCommandValidatorTests()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new RandomNumericSequenceGenerator(1, 100));
        }

        [Fact]
        public void Validate_WhenNameIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateSellerCommandValidator();
            var command = new CreateSellerCommand()
            {
                Name = string.Empty,
                EmailAddress = _faker.Internet.Email(),
                PhoneNumber = _faker.Phone.PhoneNumber("+###########")
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Name");
        }
        [Fact]
        public void Validate_WhenAllPropertiesAreValid_Success()
        {
            // Arrange
            var validator = new CreateSellerCommandValidator();
            var command = new CreateSellerCommand()
            {
                Name = _faker.Name.FullName(),
                EmailAddress = _faker.Internet.Email(),
                PhoneNumber = _faker.Phone.PhoneNumber("+###########")
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
            var validator = new CreateSellerCommandValidator();
            var command = new CreateSellerCommand()
            {
                Name = _faker.Name.FullName(),
                EmailAddress = "invalid-email",
                PhoneNumber = _faker.Phone.PhoneNumber("+###########")
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
            var validator = new CreateSellerCommandValidator();
            var command = new CreateSellerCommand()
            {
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
        public void Validate_WhenNameIsWhitespace_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateSellerCommandValidator();
            var command = new CreateSellerCommand()
            {
                Name = "   ",
                EmailAddress = _faker.Internet.Email(),
                PhoneNumber = _faker.Phone.PhoneNumber("+###########")
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Name");
        }
        [Fact]
        public void Validate_WhenEmailAddressIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateSellerCommandValidator();
            var command = new CreateSellerCommand()
            {
                Name = _faker.Name.FullName(),
                EmailAddress = string.Empty,
                PhoneNumber = _faker.Phone.PhoneNumber("+###########")
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
            var validator = new CreateSellerCommandValidator();
            var command = new CreateSellerCommand()
            {
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
        public void Validate_WhenEmailAddressIsWhitespace_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateSellerCommandValidator();
            var command = new CreateSellerCommand()
            {
                Name = _faker.Name.FullName(),
                EmailAddress = "   ",
                PhoneNumber = _faker.Phone.PhoneNumber("+###########")
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "EmailAddress");
        }
        [Fact]
        public void Validate_WhenPhoneNumberIsWhitespace_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateSellerCommandValidator();
            var command = new CreateSellerCommand()
            {
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
    }
}
