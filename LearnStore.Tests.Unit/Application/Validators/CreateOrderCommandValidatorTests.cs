using LearnStore.Application.Commands;
using LearnStore.Application.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Validators
{
    public class CreateOrderCommandValidatorTests
    {
        private readonly Fixture _fixture;
        private readonly Faker _faker = new();

        public CreateOrderCommandValidatorTests()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new RandomNumericSequenceGenerator(1, 100));
        }

        [Fact]
        public void Validate_WhenCustomerIsNull_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateOrderCommandValidator();
            var command = new CreateOrderCommand()
            {
                Customer = null,
                Items = [new OrderItemDto
              {
               Id = 1,
               Product = new ProductDto
               {
                Id = 1,
               },
               Quantity = 2
              }]
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e=> e.PropertyName == "Customer");
        }
        [Fact]
        public void Validate_WhenCustomerIdIsInvalid_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateOrderCommandValidator();
            var command = new CreateOrderCommand()
            {
                Customer = new()
                {
                    Id = 0
                },
                Items = [new OrderItemDto
              {
               Id = 1,
               Product = new ProductDto
               {
                Id = 1,
               },
               Quantity = 2
              }]
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Customer.Id");
        }

        [Fact]
        public void Validate_WhenItemsAreEmpty_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateOrderCommandValidator();
            var command = new CreateOrderCommand()
            {
                Customer = new()
                {
                    Id = 1
                },
                Items = []
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Items");
        }
        [Fact]
        public void Validate_WhenOrderItemHasInvalidQuantity_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateOrderCommandValidator();
            var command = new CreateOrderCommand()
            {
                Customer = new()
                {
                    Id = 1
                },
                Items = [new OrderItemDto
              {
               Id = 1,
               Product = new ProductDto
               {
                Id = 1,
               },
               Quantity = 0
              }]
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Items[0].Quantity");
        }

        [Fact]
        public void Validate_WhenOrderItemHasNullProduct_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateOrderCommandValidator();
            var command = new CreateOrderCommand()
            {
                Customer = new()
                {
                    Id = 1
                },
                Items = [new OrderItemDto
              {
               Id = 1,
               Product = null,
               Quantity = 2
              }]
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Items[0].Product");
        }
        [Fact]
        public void Validate_WhenOrderItemHasInvalidProductId_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateOrderCommandValidator();
            var command = new CreateOrderCommand()
            {
                Customer = new()
                {
                    Id = 1
                },
                Items = [new OrderItemDto
              {
               Id = 1,
               Product = new ProductDto
               {
                Id = 0,
               },
               Quantity = 2
              }]
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Items[0].Product.Id");
        }
        [Fact]
        public void Validate_WhenOrderItemHasInvalidId_ReturnsValidationErrors()
        {
            // Arrange
            var validator = new CreateOrderCommandValidator();
            var command = new CreateOrderCommand()
            {
                Customer = new()
                {
                    Id = 1
                },
                Items = [new OrderItemDto
              {
               Id = 0,
               Product = new ProductDto
               {
                Id = 1,
               },
               Quantity = 2
              }]
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Items[0].Id");
        }
       
        [Fact]
        public void Validate_WhenCommandIsValid_Success()
        {
            // Arrange
            var validator = new CreateOrderCommandValidator();
            var command = new CreateOrderCommand()
            {
                Customer = new()
                {
                    Id = 1
                },
                Items = [new OrderItemDto
              {
               Id = 1,
               Product = new ProductDto
               {
                Id = 1,
               },
               Quantity = 2
              }]
            };
            // Act
            var result = validator.Validate(command);
            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

    }
}
