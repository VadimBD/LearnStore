using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Validators
{
    public class CreateProductCategoryCommandValidatorTests
    {
        private readonly Fixture _fixture;
        private readonly Faker _faker = new();
        public CreateProductCategoryCommandValidatorTests()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new RandomNumericSequenceGenerator(1, 100));
        }

        [Fact]
        public void Validate_WhenNameIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateProductCategoryCommandValidator();
            var command = new CreateProductCategoryCommand()
            {
                Name = string.Empty,
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Name");
        }
        [Fact]
        public void Validate_WhenNameIsValid_Success()
        {
            // Arrange
            var validator = new CreateProductCategoryCommandValidator();
            var command = new CreateProductCategoryCommand()
            {
                Name = "Category",
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
