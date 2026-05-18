
using LearnStore.Application.Mappers;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Mappers
{
    public class ProductMapperTests
    {
        
        private IMapperRegistry CreateRegistry()
        {
            return CreateRegistry(null!);
        }

        private IMapperRegistry CreateRegistry(Action<Dictionary<Type, IMapper>> configureMappers)
        {
            var mappers = CreateMappers();
            configureMappers?.Invoke(mappers);
            var registry = Substitute.For<IMapperRegistry>();

            registry.Get<Author, AuthorDto>().Returns(mappers[typeof(IMapper<Author, AuthorDto>)]);
            registry.Get<Seller, SellerDto>().Returns(mappers[typeof(IMapper<Seller, SellerDto>)]);

            registry.Get<OrderItem, OrderItemDto>().Returns(mappers[typeof(IMapper<OrderItem, OrderItemDto>)]);

            return registry;
        }

        private Dictionary<Type, IMapper> CreateMappers()
        {
            var mappers = new Dictionary<Type, IMapper>();

            var authorMapper = Substitute.For<IMapper<Author, AuthorDto>>();
            authorMapper.ToDomain(Arg.Any<AuthorDto>()).Returns(new Author());
            authorMapper.ToDto(Arg.Any<Author>()).Returns(new AuthorDto());
            mappers[typeof(IMapper<Author, AuthorDto>)] = authorMapper;

            var sellerMapper = Substitute.For<IMapper<Seller, SellerDto>>();
            sellerMapper.ToDomain(Arg.Any<SellerDto>()).Returns(new Seller());
            sellerMapper.ToDto(Arg.Any<Seller>()).Returns(new SellerDto());
            mappers[typeof(IMapper<Seller, SellerDto>)] = sellerMapper;

            var categoryMapper = Substitute.For<IMapper<ProductCategory, ProductCategoryDto>>();
            categoryMapper.ToDomain(Arg.Any<ProductCategoryDto>()).Returns(new ProductCategory());
            categoryMapper.ToDto(Arg.Any<ProductCategory>()).Returns(new ProductCategoryDto());
            mappers[typeof(IMapper<ProductCategory, ProductCategoryDto>)] = categoryMapper;

            var orderItemMapper = Substitute.For<IMapper<OrderItem, OrderItemDto>>();
            orderItemMapper.ToDomain(Arg.Any<OrderItemDto>()).Returns(new OrderItem());
            orderItemMapper.ToDto(Arg.Any<OrderItem>()).Returns(new OrderItemDto());
            mappers[typeof(IMapper<OrderItem, OrderItemDto>)] = orderItemMapper;

            return mappers;
        }

        [Fact]
        public void ToDto_WhenProductIsNull_ThrowsArgumentNullException()
        {
           
            var registry = CreateRegistry();
            var mapper = new ProductMapper(registry);
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
                Seller = new() { Id="1"},
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
                Seller = new() { Id = "1" },
                Category = new() { Id = 1 },
                IsActive = product.IsActive,
                ChildProducts = [new() { Id = 1 }],
                Description = product.Description,
                Price = product.Price
            };
            // Act

            var registry = CreateRegistry();
            var authorMapper = registry.Get<Author, AuthorDto>();
            authorMapper.ToDto(product.Author).Returns(expectedDto.Author);
            var sellerMapper = registry.Get<Seller, SellerDto>();
            sellerMapper.ToDto(product.Seller).Returns(expectedDto.Seller);
            var categoryMapper = registry.Get<ProductCategory, ProductCategoryDto>();
            categoryMapper.ToDto(product.Category).Returns(expectedDto.Category);
            var mapper = new ProductMapper(registry);

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
            var registry = CreateRegistry();
            var mapper = new ProductMapper(registry);
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
                Seller = new() { Id = "1" },
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
                Seller = new() { Id = "1" },
                Category = new() { Id = 1 },
                IsActive = productDto.IsActive,
                ChildProducts = [new() { Id = 1 }],
                Description = productDto.Description,
                Price = productDto.Price
            };
            // Act
            var registry = CreateRegistry();
            var mapper = new ProductMapper(registry);
            var authorMapper = registry.Get<Author, AuthorDto>();
            authorMapper.ToDomain(productDto.Author).Returns(expectedDomain.Author);
            var sellerMapper = registry.Get<Seller, SellerDto>();
            sellerMapper.ToDomain(productDto.Seller).Returns(expectedDomain.Seller);
            var categoryMapper = registry.Get<ProductCategory, ProductCategoryDto>();
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
