using LearnStore.Application.Commands.ProductCategoryCommands;
using LearnStore.Application.Validators.ProductCategoryValidators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace LearnStore.Tests.Unit.Application.Validators
{
    public class UpdateProductCategoryCommandValidatorTests
    {
        private readonly Fixture _fixture;
        private readonly Faker _faker = new();

        public UpdateProductCategoryCommandValidatorTests()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new RandomNumericSequenceGenerator(1, 100));
        }
        [Fact]
        public void Validate_WhenNameIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateProductCategoryCommandValidator();
            var command = new UpdateProductCategoryCommand()
            {
                Id = 10,
                Name = string.Empty
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
            var validator = new UpdateProductCategoryCommandValidator();
            var command = new UpdateProductCategoryCommand()
            {
                Id = 10,
                Name = "Category"
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
        [Fact]
        public void Validate_WhenIdIsZeroOrNegative_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateProductCategoryCommandValidator();
            var command = new UpdateProductCategoryCommand()
            {
                Id = 0,
                Name = "Category1"
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Id");
        }
        [Fact]
        public void Validate_WhenNameIsWhitespace_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateProductCategoryCommandValidator();
            var command = new UpdateProductCategoryCommand()
            {
                Id = 10,
                Name = "   "
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Name");
        }
    }
}
