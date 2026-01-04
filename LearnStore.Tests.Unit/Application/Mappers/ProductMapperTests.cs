using LearnStore.Application.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Mappers
{
    public class ProductMapperTests
    {
        private readonly Faker _faker = new();
        private readonly Fixture _fixture = new();
        [Fact]
        public void ToDto_WhenProductIsNull_ThrowsArgumentNullException()
        {
            var mapper = new ProductMapper();
            // Act
            Action action = () => mapper.ToDto(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("product");
        }
        [Fact]
        public void ToDto_WhenProductIsValid_ReturnsExpectedDto()
        {
            var mapper = new ProductMapper();
            var authorMapper = new AuthorMapper();
            var sellerMapper = new SellerMapper();
            var categoryMapper = new ProductCategoryMapper();
            // Arrange
            var product = new Product()
            {
                Id = _faker.Random.Int(),
                Name = _faker.Commerce.ProductName(),
                Author = new() { Id=1},
                Seller = new() { Id=1},
                Category = new() { Id=1},
                IsActive = true,
                ChildProducts = [new() {Id=1}],
                Description = _faker.Commerce.ProductDescription(),
                Price = decimal.Parse(_faker.Commerce.Price(1, 1000))
            };
            var expectedDto = new ProductDto
            {
               Id = product.Id,
                Name = product.Name,
                Author = authorMapper.ToDto(product.Author),
                Seller = sellerMapper.ToDto(product.Seller),
                Category = categoryMapper.ToDto(product.Category),
                IsActive = product.IsActive,
                ChildProducts = [..product.ChildProducts.Select(cp => mapper.ToDto(cp))],
                Description = product.Description,
                Price = product.Price
            };
            // Act
            var result = mapper.ToDto(product);
            // Assert
            result.Should().BeEquivalentTo(expectedDto);
        }
        [Fact]
        public void ToEntity_WhenProductDtoIsNull_ThrowsArgumentNullException()
        {
            var mapper = new ProductMapper();
            // Act
            Action action = () => mapper.ToEntity(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("productDto");
        }
        [Fact]
        public void ToEntity_WhenProductDtoIsValid_ReturnsExpectedEntity()
        {
            var mapper = new ProductMapper();
            var authorMapper = new AuthorMapper();
            var sellerMapper = new SellerMapper();
            var categoryMapper = new ProductCategoryMapper();
            // Arrange
            var productDto = new ProductDto()
            {
                Id = _faker.Random.Int(),
                Name = _faker.Commerce.ProductName(),
                Author = new() { Id = 1 },
                Seller = new() { Id = 1 },
                Category = new() { Id = 1 },
                IsActive = true,
                ChildProducts = [new() { Id = 1 }],
                Description = _faker.Commerce.ProductDescription(),
                Price = decimal.Parse(_faker.Commerce.Price(1, 1000))
            };
            var expectedEntity = new Product()
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Author = authorMapper.ToEntity(productDto.Author),
                Seller = sellerMapper.ToEntity(productDto.Seller),
                Category = categoryMapper.ToEntity(productDto.Category),
                IsActive = productDto.IsActive,
                ChildProducts = [.. productDto.ChildProducts.Select(cp => mapper.ToEntity(cp))],
                Description = productDto.Description,
                Price = productDto.Price
            };
            // Act
            var result = mapper.ToEntity(productDto);
            // Assert
            result.Should().BeEquivalentTo(expectedEntity);
        }
    }
}
