using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Validators
{
    public class UpdateOrderStateCommandValidatorTests
    {
        private readonly Fixture _fixture;
        private readonly Faker _faker = new();
        public UpdateOrderStateCommandValidatorTests()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new RandomNumericSequenceGenerator(1, 100));
        }
        [Fact]
        public void Validate_WhenIdIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateOrderStateCommandValidator();
            var command = new UpdateOrderStateCommand()
            {
                OrderId = Guid.Empty,
                OrderState = OrderState.Pending

            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "OrderId");
        }
        [Fact]
        public void Validate_WhenAllPropertiesAreValid_Success()
        {
            // Arrange
            var validator = new UpdateOrderStateCommandValidator();
            var command = new UpdateOrderStateCommand()
            {
                OrderId = Guid.NewGuid(),
                OrderState = OrderState.Pending
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
        [Fact]
        public void Validate_WhenOrderStateIsInvalid_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateOrderStateCommandValidator();
            var command = new UpdateOrderStateCommand()
            {
                OrderId = Guid.NewGuid(),
                OrderState = (OrderState)999 // Invalid state
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "OrderState");
        }
    }
}
