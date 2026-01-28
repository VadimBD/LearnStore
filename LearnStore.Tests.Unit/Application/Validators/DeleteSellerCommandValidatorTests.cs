using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Validators
{
    public class DeleteSellerCommandValidatorTests
    {
        private readonly Fixture _fixture;
        private readonly Faker _faker = new();
        public DeleteSellerCommandValidatorTests()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new RandomNumericSequenceGenerator(1, 100));
        }
        [Fact]
        public void Validate_WhenIdIsInvalid_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new DeleteSellerCommandValidator();
            var command = new DeleteSellerCommand(0);
            
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "SellerId");
        }
        [Fact]
        public void Validate_WhenIdIsValid_Success()
        {
            // Arrange
            var validator = new DeleteSellerCommandValidator();
            var command = new DeleteSellerCommand(10);
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
