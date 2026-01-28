using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Validators
{
    public class CreatePaymentCommandValidatorTests
    {
        private readonly Fixture _fixture;
        private readonly Faker _faker = new();
        public CreatePaymentCommandValidatorTests()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new RandomNumericSequenceGenerator(1, 100));
        }

        [Fact]
        public void Validate_WhenAmountIsLessThanOrEqualToZero_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreatePaymentCommandValidator();
            var command = new CreatePaymentCommand()
            {
                Amount = 0m,
                OrderId = Guid.NewGuid(),
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Amount");
        }
        [Fact]
        public void Validate_WhenOrderIdIsEmptyGuid_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreatePaymentCommandValidator();
            var command = new CreatePaymentCommand()
            {
                Amount = 100.00m,
                OrderId = Guid.Empty,
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "OrderId");
        }
        [Fact]
        public void Validate_WhenAmountIsNegative_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreatePaymentCommandValidator();
            var command = new CreatePaymentCommand()
            {
                Amount = -50.00m,
                OrderId = Guid.NewGuid(),
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Amount");
        }

        [Fact]
        public void Validate_WhenAllPropertiesAreValid_Success()
        {
            // Arrange
            var validator = new CreatePaymentCommandValidator();
            var command = new CreatePaymentCommand()
            {
                Amount = 100.00m,
                OrderId = Guid.NewGuid(),
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
