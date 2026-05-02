using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.UseCases
{
    public class GetProductsHandlerTests
    {

        private readonly Fixture _fixture = new();

        public IMapper<Product, ProductDto> GetProductMapper()
        {
            var productMapper = Substitute.For<IMapper<Product, ProductDto>>();
            productMapper
                .ToDto(Arg.Any<Product>())
            .Returns(call =>
            {
                var p = call.Arg<Product>();
                return new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Category = new ProductCategoryDto
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name
                    },
                    Seller = p.Seller == null ? null : new SellerDto
                    {
                        Id = p.Seller.Id,
                        Name = p.Seller.Name
                    },
                    IsActive = p.IsActive
                };

            });
            return productMapper;
        }

        [Fact]
        public async Task Handle_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            var productRepository = Substitute.For<IProductRepository>();
            var productMapper = Substitute.For<IMapper<Product, ProductDto>>();
            var handler = new GetProductsHandler(productRepository, productMapper);

            Func<Task> act = async () => await handler.Handle(null!, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("query");
        }

        [Fact]
        public async Task Handle_WhenCategoryIdMatches_ReturnsProducts()
        {
            var productRepository = Substitute.For<IProductRepository>();

            var ctegoryId = 1;

            productRepository.Products.Returns([
                new Product { Id = 1, Name = "Product1", Description = "Test Description", Price = 10.0m, Category=new ProductCategory { Id = ctegoryId, Name = "Category1" } },
                new Product { Id = 2, Name = "Product2", Description = "Test Description", Price = 20.0m, Category=new ProductCategory { Id = 2, Name = "Category2" } },
                new Product { Id = 3, Name = "Product3", Description = "Test Description", Price = 30.0m, Category=new ProductCategory { Id = ctegoryId, Name = "Category1" } },
                new Product { Id = 4, Name = "Product4", Description = "Test Description", Price = 40.0m, Category=new ProductCategory { Id = 3, Name = "Category3" } }
            ]);

            var productMapper = GetProductMapper();
            var handler = new GetProductsHandler(productRepository, productMapper);
            var query = new GetProductsQuery { CategoryId = ctegoryId };

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().HaveCount(2);
            result.All(p => p.Category.Id == ctegoryId).Should().BeTrue();
        }

        [Fact]
        public async Task Handle_WhenNameContainsFilter_ReturnsProducts()
        {
            var productRepository = Substitute.For<IProductRepository>();
            productRepository.Products.Returns([
                new Product { Id = 1, Name = "Apple iPhone", Description = "Test Description", Price = 10.0m, Category=new ProductCategory { Id = 1, Name = "Category1" } },
                new Product { Id = 2, Name = "Samsung Galaxy", Description = "Test Description", Price = 20.0m, Category=new ProductCategory { Id = 2, Name = "Category2" } },
                new Product { Id = 3, Name = "Google Pixel", Description = "Test Description", Price = 30.0m, Category=new ProductCategory { Id = 1, Name = "Category1" } },
                new Product { Id = 4, Name = "Apple MacBook", Description = "Test Description", Price = 40.0m, Category=new ProductCategory { Id = 3, Name = "Category3" } }
                ]);
            var productMapper = GetProductMapper();
            var handler = new GetProductsHandler(productRepository, productMapper);
            var query = new GetProductsQuery { ProductName = "Apple" };
            var result = await handler.Handle(query, CancellationToken.None);
            result.Should().HaveCount(2);
            result.All(p => p.Name.Contains("Apple")).Should().BeTrue();
        }

        [Fact]
        async Task Handle_WhenIsAvailableMatches_ReturnsProducts()
        {
           var productRepository = Substitute.For<IProductRepository>();
            productRepository.Products.Returns([
                new Product { Id = 1, Name = "Product1", Description = "Test Description", Price = 10.0m, IsActive=true, Category=new ProductCategory { Id = 1, Name = "Category1" } },
                new Product { Id = 2, Name = "Product2", Description = "Test Description", Price = 20.0m, IsActive=false, Category=new ProductCategory { Id = 2, Name = "Category2" } },
                new Product { Id = 3, Name = "Product3", Description = "Test Description", Price = 30.0m, IsActive=true, Category=new ProductCategory { Id = 1, Name = "Category1" } },
                new Product { Id = 4, Name = "Product4", Description = "Test Description", Price = 40.0m, IsActive=false, Category=new ProductCategory { Id = 3, Name = "Category3" } }
                ]);
            var productMapper = GetProductMapper();
            var handler = new GetProductsHandler(productRepository, productMapper);
            var query = new GetProductsQuery { IsActive = true };
            var result = await handler.Handle(query, CancellationToken.None);
            result.Should().HaveCount(2);
            result.All(p => p.IsActive == true).Should().BeTrue();
        }

        [Fact]
        public async Task Handle_WhenPriceMatches_ReturnsProducts()
        {
            var productRepository = Substitute.For<IProductRepository>();
                productRepository.Products.Returns([
                    new Product { Id = 1, Name = "Product1", Description = "Test Description", Price = 10.0m, Category=new ProductCategory { Id = 1, Name = "Category1" } },
                    new Product { Id = 2, Name = "Product2", Description = "Test Description", Price = 20.0m, Category=new ProductCategory { Id = 2, Name = "Category2" } },
                    new Product { Id = 3, Name = "Product3", Description = "Test Description", Price = 30.0m, Category=new ProductCategory { Id = 1, Name = "Category1" } },
                    new Product { Id = 4, Name = "Product4", Description = "Test Description", Price = 40.0m, Category=new ProductCategory { Id = 3, Name = "Category3" } }
                ]);
            var productMapper = GetProductMapper();
            var handler = new GetProductsHandler(productRepository, productMapper);
            var query = new GetProductsQuery { Price = 20.0m };
            var result = await handler.Handle(query, CancellationToken.None);
            result.Should().HaveCount(2);
            result.All(p => p.Price <= 20.0m).Should().BeTrue();
        }

        

        [Fact]
        public async Task Handle_WhenNoFiltersApplied_ReturnsAllProducts()
        {
            var productRepository = Substitute.For<IProductRepository>();
            productRepository.Products.Returns([
                new Product { Id = 1, Name = "Product1", Description = "Test Description", Price = 10.0m, Category=new ProductCategory { Id = 1, Name = "Category1" } },
                new Product { Id = 2, Name = "Product2", Description = "Test Description", Price = 20.0m, Category=new ProductCategory { Id = 2, Name = "Category2" } },
                new Product { Id = 3, Name = "Product3", Description = "Test Description", Price = 30.0m, Category=new ProductCategory { Id = 1, Name = "Category1" } },
                new Product { Id = 4, Name = "Product4", Description = "Test Description", Price = 40.0m, Category=new ProductCategory { Id = 3, Name = "Category3" } }
            ]);
            var productMapper = GetProductMapper();
            var handler = new GetProductsHandler(productRepository, productMapper);
            var query = new GetProductsQuery { };
            var result = await handler.Handle(query, CancellationToken.None);
            result.Should().HaveCount(4);
        }

        [Fact]
        public async Task Handle_WhenAllFiltersApplied_ReturnsProducts()
        {
            var productRepository = Substitute.For<IProductRepository>();
            productRepository.Products.Returns([
                new Product { Id = 1, Name = "Apple iPhone", Description = "Test Description", Price = 10.0m, IsActive=true, Category=new ProductCategory { Id = 1, Name = "Category1" },Seller=new Seller { Id = 1, Name = "Seller1" } },
                new Product { Id = 2, Name = "Samsung Galaxy", Description = "Test Description", Price = 20.0m, IsActive=false, Category=new ProductCategory { Id = 2, Name = "Category2" },Seller=new Seller { Id = 2, Name = "Seller2" } },
                new Product { Id = 3, Name = "Google Pixel", Description = "Test Description", Price = 30.0m, IsActive=true, Category=new ProductCategory { Id = 1, Name = "Category1" },Seller=new Seller { Id = 3, Name = "Seller3" } },
                new Product { Id = 4, Name = "Apple MacBook", Description = "Test Description", Price = 40.0m, IsActive=false, Category=new ProductCategory { Id = 3, Name = "Category3" },Seller=new Seller { Id = 4, Name = "Seller4" } }
            ]);
            var productMapper = GetProductMapper();
            var handler = new GetProductsHandler(productRepository, productMapper);
            var query = new GetProductsQuery { 
                CategoryId = 1,
                ProductName = "Apple", 
                IsActive = true, 
                Price = 10.0m ,
                SellerId = 1
            };
            var result = await handler.Handle(query, CancellationToken.None);
            result.Should().HaveCount(1);
           result.First().Id.Should().Be(1);
        }


    }
}
