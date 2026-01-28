using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Validators
{
    public class CreateProductCommandValidatorTests
    {
        private readonly Fixture _fixture;
        private readonly Faker _faker = new();
        public CreateProductCommandValidatorTests()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new RandomNumericSequenceGenerator(1, 100));
        }
        [Fact]
        public void Validate_WhenNameIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateProductCommandValidator();
            var command = new CreateProductCommand()
            {
                Name = string.Empty,
                Description = "Test",
                Price = 10.00m,
                Author = new() { Id=1},
                Seller = new() { Id = 1 },
                Category = new() { Id = 1 },
                ChildProducts = [new() { Id=2}],
                IsActive = true
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
            var validator = new CreateProductCommandValidator();
            var command = new CreateProductCommand()
            {
                Name = "ProductName",
                Description = "Test",
                Price = 10.00m,
                Author = new() { Id = 1 },
                Seller = new() { Id = 1 },
                Category = new() { Id = 1 },
                ChildProducts = [new() { Id = 2 }],
                IsActive = true
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();

        }
        [Fact]
        public void Validate_WhenPriceIsNegative_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateProductCommandValidator();
            var command = new CreateProductCommand()
            {
                Name = "ProductName",
                Description = "Test",
                Price = -5.00m,
                Author = new() { Id = 1 },
                Seller = new() { Id = 1 },
                Category = new() { Id = 1 },
                ChildProducts = [new() { Id = 2 }],
                IsActive = true
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Price");
        }
        [Fact]
        public void Validate_WhenAuthorIsNull_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateProductCommandValidator();
            var command = new CreateProductCommand()
            {
                Name = "ProductName",
                Description = "Test",
                Price = 10.00m,
                Author = null,
                Seller = new() { Id = 1 },
                Category = new() { Id = 1 },
                ChildProducts = [new() { Id = 2 }],
                IsActive = true
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Author");
        }
        [Fact]
        public void Validate_WhenSellerIsNull_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateProductCommandValidator();
            var command = new CreateProductCommand()
            {
                Name = "ProductName",
                Description = "Test",
                Price = 10.00m,
                Author = new() { Id = 1 },
                Seller = null,
                Category = new() { Id = 1 },
                ChildProducts = [new() { Id = 2 }],
                IsActive = true
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Seller");
        }
        [Fact]
        public void Validate_WhenCategoryIsNull_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateProductCommandValidator();
            var command = new CreateProductCommand()
            {
                Name = "ProductName",
                Description = "Test",
                Price = 10.00m,
                Author = new() { Id = 1 },
                Seller = new() { Id = 1 },
                Category = null,
                ChildProducts = [new() { Id = 2 }],
                IsActive = true
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Category");
        }
        [Fact]
        public void Validate_WhenChildProductsIsNull_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateProductCommandValidator();
            var command = new CreateProductCommand()
            {
                Name = "ProductName",
                Description = "Test",
                Price = 10.00m,
                Author = new() { Id = 1 },
                Seller = new() { Id = 1 },
                Category = new() { Id = 1 },
                ChildProducts = null!,
                IsActive = true
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "ChildProducts");
        }
        [Fact]
        public void Validate_WhenDescriptionIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateProductCommandValidator();
            var command = new CreateProductCommand()
            {
                Name = "ProductName",
                Description = string.Empty,
                Price = 10.00m,
                Author = new() { Id = 1 },
                Seller = new() { Id = 1 },
                Category = new() { Id = 1 },
                ChildProducts = [new() { Id = 2 }],
                IsActive = true
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Description");
        }
        [Fact]
        public void Validate_WhenNameIsWhitespace_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateProductCommandValidator();
            var command = new CreateProductCommand()
            {
                Name = "   ",
                Description = "Test",
                Price = 10.00m,
                Author = new() { Id = 1 },
                Seller = new() { Id = 1 },
                Category = new() { Id = 1 },
                ChildProducts = [new() { Id = 2 }],
                IsActive = true
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Name");
        }
        [Fact]
        public void Validate_WhenDescriptionIsWhitespace_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateProductCommandValidator();
            var command = new CreateProductCommand()
            {
                Name = "ProductName",
                Description = "   ",
                Price = 10.00m,
                Author = new() { Id = 1 },
                Seller = new() { Id = 1 },
                Category = new() { Id = 1 },
                ChildProducts = [new() { Id = 2 }],
                IsActive = true
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Description");
        }
        [Fact]
        public void Validate_WhenPriceIsZero_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateProductCommandValidator();
            var command = new CreateProductCommand()
            {
                Name = "ProductName",
                Description = "Test",
                Price = 0.00m,
                Author = new() { Id = 1 },
                Seller = new() { Id = 1 },
                Category = new() { Id = 1 },
                ChildProducts = [new() { Id = 2 }],
                IsActive = true
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Price");
        }
        [Fact]
        public void Validate_WhenChildProductsIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateProductCommandValidator();
            var command = new CreateProductCommand()
            {
                Name = "ProductName",
                Description = "Test",
                Price = 10.00m,
                Author = new() { Id = 1 },
                Seller = new() { Id = 1 },
                Category = new() { Id = 1 },
                ChildProducts = [],
                IsActive = true
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "ChildProducts");
        }
        [Fact]
        public void Validate_WhenAllPropertiesAreNull_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateProductCommandValidator();
            var command = new CreateProductCommand()
            {
                Name = null!,
                Description = null!,
                Price = 10.00m,
                Author = null,
                Seller = null,
                Category = null,
                ChildProducts = null!,
                IsActive = true
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
            result.Errors.Should().Contain(e => e.PropertyName == "Description");
            result.Errors.Should().Contain(e => e.PropertyName == "Author");
            result.Errors.Should().Contain(e => e.PropertyName == "Seller");
            result.Errors.Should().Contain(e => e.PropertyName == "Category");
            result.Errors.Should().Contain(e => e.PropertyName == "ChildProducts");
        }
    }
}
