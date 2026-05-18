using LearnStore.Application.Commands.ProductCommands;
using LearnStore.Application.Validators.ProductValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Validators
{
    public class UpdateProductCommandValidatorTests
    {
        private readonly Fixture _fixture;
        private readonly Faker _faker = new();
        public UpdateProductCommandValidatorTests()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new RandomNumericSequenceGenerator(1, 100));
        }
        [Fact]
        public void Validate_WhenNameIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateProductCommandValidator();
            var command = new UpdateProductCommand()
            {
                Id = 10,
                Name = string.Empty,
                Description = _faker.Lorem.Sentence(),
                Price = _faker.Random.Decimal(1, 100),
                IsActive = true,

                Author = new (){ Id = 10,},
                Seller = new (){ Id = 10,},
                Category = new (){ Id = 10,},
                ChildProducts = [new() { Id=2}],
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
            var validator = new UpdateProductCommandValidator();
            var command = new UpdateProductCommand()
            {
                Id = 10,
                Name = "ProductName",
                Description = _faker.Lorem.Sentence(),
                Price = _faker.Random.Decimal(1, 100),
                IsActive = true,
                Author = new (){ Id = 10,},
                Seller = new (){ Id = 10,},
                Category = new (){ Id = 10,},
                ChildProducts = [new() { Id=2}],
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
        [Fact]
        public void Validate_WhenPriceIsInvalid_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateProductCommandValidator();
            var command = new UpdateProductCommand()
            {
                Id = 10,
                Name = "ProductName",
                Description = _faker.Lorem.Sentence(),
                Price = 0, // Invalid price
                IsActive = true,
                Author = new (){ Id = 10,},
                Seller = new (){ Id = 10,},
                Category = new (){ Id = 10,},
                ChildProducts = [new() { Id=2}],
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
            var validator = new UpdateProductCommandValidator();
            var command = new UpdateProductCommand()
            {
                Id = 10,
                Name = "ProductName",
                Description = _faker.Lorem.Sentence(),
                Price = _faker.Random.Decimal(1, 100),
                IsActive = true,
                Author = null, // Invalid Author
                Seller = new (){ Id = 10,},
                Category = new (){ Id = 10,},
                ChildProducts = [new() { Id=2}],
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Author");
        }
        [Fact]
        public void Validate_WhenChildProductIdIsInvalid_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateProductCommandValidator();
            var command = new UpdateProductCommand()
            {
                Id = 10,
                Name = "ProductName",
                Description = _faker.Lorem.Sentence(),
                Price = _faker.Random.Decimal(1, 100),
                IsActive = true,
                Author = new (){ Id = 10,},
                Seller = new (){ Id = 10,},
                Category = new (){ Id = 10,},
                ChildProducts = [new() { Id=0}], // Invalid Child Product Id
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "ChildProducts[0].Id");
        }
        [Fact]
        public void Validate_WhenCategoryIsNull_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateProductCommandValidator();
            var command = new UpdateProductCommand()
            {
                Id = 10,
                Name = "ProductName",
                Description = _faker.Lorem.Sentence(),
                Price = _faker.Random.Decimal(1, 100),
                IsActive = true,
                Author = new (){ Id = 10,},
                Seller = new (){ Id = 10,},
                Category = null, // Invalid Category
                ChildProducts = [new() { Id=2}],
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Category");
        }
        [Fact]
        public void Validate_WhenSellerIsNull_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateProductCommandValidator();
            var command = new UpdateProductCommand()
            {
                Id = 10,
                Name = "ProductName",
                Description = _faker.Lorem.Sentence(),
                Price = _faker.Random.Decimal(1, 100),
                IsActive = true,
                Author = new (){ Id = 10,},
                Seller = null, // Invalid Seller
                Category = new (){ Id = 10,},
                ChildProducts = [new() { Id=2}],
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Seller");
        }
        
        [Fact]
        public void Validate_WhenIdIsInvalid_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateProductCommandValidator();
            var command = new UpdateProductCommand()
            {
                Id = 0, // Invalid Id
                Name = "ProductName",
                Description = _faker.Lorem.Sentence(),
                Price = _faker.Random.Decimal(1, 100),
                IsActive = true,
                Author = new (){ Id = 10,},
                Seller = new (){ Id = 10,},
                Category = new (){ Id = 10,},
                ChildProducts = [new() { Id=2}],
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Id");
        }
        [Fact]
        public void Validate_WhenDescriptionIsEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new UpdateProductCommandValidator();
            var command = new UpdateProductCommand()
            {
                Id = 10,
                Name = "ProductName",
                Description = string.Empty,
                Price = _faker.Random.Decimal(1, 100),
                IsActive = true,
                Author = new (){ Id = 10,},
                Seller = new (){ Id = 10,},
                Category = new (){ Id = 10,},
                ChildProducts = [new() { Id=2}],
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
            var validator = new UpdateProductCommandValidator();
            var command = new UpdateProductCommand()
            {
                Id = 10,
                Name = "   ",
                Description = _faker.Lorem.Sentence(),
                Price = _faker.Random.Decimal(1, 100),
                IsActive = true,
                Author = new (){ Id = 10,},
                Seller = new (){ Id = 10,},
                Category = new (){ Id = 10,},
                ChildProducts = [new() { Id=2}],
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
            var validator = new UpdateProductCommandValidator();
            var command = new UpdateProductCommand()
            {
                Id = 10,
                Name = "ProductName",
                Description = "   ",
                Price = _faker.Random.Decimal(1, 100),
                IsActive = true,
                Author = new (){ Id = 10,},
                Seller = new (){ Id = 10,},
                Category = new (){ Id = 10,},
                ChildProducts = [new() { Id=2}],
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Description");
        }    
    }
}
