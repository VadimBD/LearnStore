using LearnStore.Application.Commands.SellerCommands;
using LearnStore.Application.Validators.SellerValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Validators
{
    public class UpdateSellerCommandValidatorTests
    {
        private readonly Fixture _fixture;
        private readonly Faker _faker = new();
        public UpdateSellerCommandValidatorTests()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new RandomNumericSequenceGenerator(1, 100));
        }
        [Fact]
        public void Validate_WhenNameIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateSellerCommandValidator();
            var command = new UpdateSellerCommand()
            {
                Id = 10,
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
        public void Validate_WhenAllPropertiesAreValid_Success()
        {
            // Arrange
            var validator = new UpdateSellerCommandValidator();
            var command = new UpdateSellerCommand()
            {
                Id = 10,
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
            var validator = new UpdateSellerCommandValidator();
            var command = new UpdateSellerCommand()
            {
                Id = 10,
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
        public void Validate_WhenIdIsZeroOrNegative_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateSellerCommandValidator();
            var command = new UpdateSellerCommand()
            {
                Id = 0,
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
        public void Validate_WhenPhoneNumberIsInvalid_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateSellerCommandValidator();
            var command = new UpdateSellerCommand()
            {
                Id = 10,
                Name = _faker.Name.FullName(),
                EmailAddress = _faker.Internet.Email(),
                PhoneNumber = "02345"
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "PhoneNumber");
        }
        [Fact]
        public void Validate_WhenPhoneNumberIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateSellerCommandValidator();
            var command = new UpdateSellerCommand()
            {
                Id = 10,
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
            var validator = new UpdateSellerCommandValidator();
            var command = new UpdateSellerCommand()
            {
                Id = 10,
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
        public void Validate_WhenPhoneNumberIsTooLong_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateSellerCommandValidator();
            var command = new UpdateSellerCommand()
            {
                Id = 10,
                Name = _faker.Name.FullName(),
                EmailAddress = _faker.Internet.Email(),
                PhoneNumber = "+380" + new string('1', 21) // Assuming max length is 20
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "PhoneNumber");
        }
        [Fact]
        public void Validate_WhenNameIsWhitespaceOnly_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateSellerCommandValidator();
            var command = new UpdateSellerCommand()
            {
                Id = 10,
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
        public void Validate_WhenEmailAddressIsWhitespaceOnly_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateSellerCommandValidator();
            var command = new UpdateSellerCommand()
            {
                Id = 10,
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
        public void Validate_WhenPhoneNumberIsWhitespaceOnly_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateSellerCommandValidator();
            var command = new UpdateSellerCommand()
            {
                Id = 10,
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
