
using LearnStore.Application.Mappers;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Mappers
{
    public class ProductMapperTests
    {
        private readonly Faker _faker = new();
       

        private IEnumerable<IMapper> CreateMappers()
        {
            var authorMapper = Substitute.For<IMapper<Author,AuthorDto>>();
            var sellerMapper = Substitute.For<IMapper<Seller,SellerDto>>();
            var categoryMapper = Substitute.For<IMapper<ProductCategory,ProductCategoryDto>>();
            return [authorMapper, sellerMapper, categoryMapper];
        }

        [Fact]
        public void ToDto_WhenProductIsNull_ThrowsArgumentNullException()
        {
            var mappers = CreateMappers();
            var mapper = new ProductMapper(mappers);
            // Act
            Action action = () => mapper.ToDto(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("product");
        }
        [Fact]
        public void ToDto_WhenProductIsValid_ReturnsExpectedDto()
        {
            // Arrange
            var product = new Product()
            {
                Id = 1,
                Name = "Product1",
                Author = new() { Id=1},
                Seller = new() { Id=1},
                Category = new() { Id=1},
                IsActive = true,
                ChildProducts = [new() {Id=1}],
                Description = "Test",
                Price = 10m
            };
            var expectedDto = new ProductDto
            {
               Id = product.Id,
                Name = product.Name,
                Author = new() { Id = 1 },
                Seller = new() { Id = 1 },
                Category = new() { Id = 1 },
                IsActive = product.IsActive,
                ChildProducts = [new() { Id = 1 }],
                Description = product.Description,
                Price = product.Price
            };
            // Act

            var mappers = CreateMappers();
            var authorMapper = mappers.OfType<IMapper<Author, AuthorDto>>().First();
            authorMapper.ToDto(product.Author).Returns(expectedDto.Author);
            var sellerMapper = mappers.OfType<IMapper<Seller, SellerDto>>().First();
            sellerMapper.ToDto(product.Seller).Returns(expectedDto.Seller);
            var categoryMapper = mappers.OfType<IMapper<ProductCategory, ProductCategoryDto>>().First();
            categoryMapper.ToDto(product.Category).Returns(expectedDto.Category);
            var mapper = new ProductMapper(mappers);

            var result = mapper.ToDto(product);
            // Assert
            result.Should().BeEquivalentTo(expectedDto);
            authorMapper.Received(1).ToDto(product.Author);
            sellerMapper.Received(1).ToDto(product.Seller);
            categoryMapper.Received(1).ToDto(product.Category);
        }
        [Fact]
        public void ToDomain_WhenProductDtoIsNull_ThrowsArgumentNullException()
        {
            var mappers = CreateMappers();
            var mapper = new ProductMapper(mappers); ;
            // Act
            Action action = () => mapper.ToDomain(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("productDto");
        }
        [Fact]
        public void ToDomain_WhenProductDtoIsValid_ReturnsexpectedDomain()
        {
            
            // Arrange
            var productDto = new ProductDto()
            {
                Id = 1,
                Name = "ProductName",
                Author = new() { Id = 1 },
                Seller = new() { Id = 1 },
                Category = new() { Id = 1 },
                IsActive = true,
                ChildProducts = [new() { Id = 1 }],
                Description = "Test",
                Price = 10m
            };
            var expectedDomain = new Product()
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Author = new() { Id = 1 },
                Seller = new() { Id = 1 },
                Category = new() { Id = 1 },
                IsActive = productDto.IsActive,
                ChildProducts = [new() { Id = 1 }],
                Description = productDto.Description,
                Price = productDto.Price
            };
            // Act
            var mappers = CreateMappers();
            var mapper = new ProductMapper(mappers);
            var authorMapper = mappers.OfType<IMapper<Author, AuthorDto>>().First();
            authorMapper.ToDomain(productDto.Author).Returns(expectedDomain.Author);
            var sellerMapper = mappers.OfType<IMapper<Seller, SellerDto>>().First();
            sellerMapper.ToDomain(productDto.Seller).Returns(expectedDomain.Seller);
            var categoryMapper = mappers.OfType<IMapper<ProductCategory, ProductCategoryDto>>().First();
            categoryMapper.ToDomain(productDto.Category).Returns(expectedDomain.Category);
            var result = mapper.ToDomain(productDto);
            // Assert
            result.Should().BeEquivalentTo(expectedDomain);
            authorMapper.Received(1).ToDomain(productDto.Author);
            sellerMapper.Received(1).ToDomain(productDto.Seller);
            categoryMapper.Received(1).ToDomain(productDto.Category);
        }
    }
}
