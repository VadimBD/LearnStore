using LearnStore.Application.Commands.OrderCommands;
using LearnStore.Application.Validators.OrderValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Validators
{
    public class DeleteOrderCommandValidatorTests
    {
        private readonly Fixture _fixture;
        private readonly Faker _faker = new();
        public DeleteOrderCommandValidatorTests()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new RandomNumericSequenceGenerator(1, 100));
        }
        [Fact]
        public void Validate_WhenIdIsInvalid_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new DeleteOrderCommandValidator();
            var command = new DeleteOrderCommand(Guid.Empty);
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "OrderId");
        }
        [Fact]
        public void Validate_WhenIdIsValid_Success()
        {
            // Arrange
            var validator = new DeleteOrderCommandValidator();
            var command = new DeleteOrderCommand(Guid.NewGuid());
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
