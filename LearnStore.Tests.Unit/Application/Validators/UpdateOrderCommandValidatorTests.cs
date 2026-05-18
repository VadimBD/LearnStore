using LearnStore.Application.Commands.OrderCommands;
using LearnStore.Application.Validators.OrderValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Validators
{
    public class UpdateOrderCommandValidatorTests
    {
        private readonly Fixture _fixture;
        private readonly Faker _faker = new();
        public UpdateOrderCommandValidatorTests()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new RandomNumericSequenceGenerator(1, 100));
        }
        [Fact]
        public void Validate_WhenIdIsInvalid_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateOrderCommandValidator();
            var command = new UpdateOrderCommand()
            {
                Id = Guid.Empty,
                Items = [new() { Id=1}],
                Customer = new() { Id = 10 },
                Payments = [new() { Id = Guid.NewGuid()}]
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
            var validator = new UpdateOrderCommandValidator();
            var command = new UpdateOrderCommand()
            {
                Id = Guid.NewGuid(),
                Items = [new() { Id=1}],
                Customer = new() { Id = 10 },
                Payments = [new() { Id = Guid.NewGuid()}]
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
        [Fact]
        public void Validate_WhenItemsAreEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateOrderCommandValidator();
            var command = new UpdateOrderCommand()
            {
                Id = Guid.NewGuid(),
                Items = [],
                Customer = new() { Id = 10 },
                Payments = [new() { Id = Guid.NewGuid()}]
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Items");
        }
        [Fact]
        public void Validate_WhenCustomerIsNull_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateOrderCommandValidator();
            var command = new UpdateOrderCommand()
            {
                Id = Guid.NewGuid(),
                Items = [new() { Id=1}],
                Customer = null,
                Payments = [new() { Id = Guid.NewGuid()}]
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Customer");
        }
        [Fact]
        public void Validate_WhenPaymentsIsNull_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateOrderCommandValidator();
            var command = new UpdateOrderCommand()
            {
                Id = Guid.NewGuid(),
                Items = [new() { Id=1}],
                Customer = new() { Id = 10 },
                Payments = null!
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Payments");
        }
        [Fact]
        public void Validate_WhenItemIdIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateOrderCommandValidator();
            var command = new UpdateOrderCommand()
            {
                Id = Guid.NewGuid(),
                Items = [],
                Customer = new() { Id = 10 },
                Payments = [new() { Id = Guid.NewGuid()}]
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Items");
        }
    }
}
