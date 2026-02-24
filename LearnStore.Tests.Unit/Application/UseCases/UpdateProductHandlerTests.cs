using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.UseCases
{
    public class UpdateProductHandlerTests
    {
        private readonly Fixture _fixture = new();
        private List<IMapper> GetMappers()
        {
            var commandMapper = Substitute.For<IMapper<Product, CreateProductCommand>>();

            commandMapper.ToDomain(Arg.Any<CreateProductCommand>())
                .Returns(new Product());

            var productMapper = Substitute.For<IMapper<Product, ProductDto>>();
            productMapper.ToDomain(Arg.Any<ProductDto>())
                .Returns(new Product());

            var authorMapper = Substitute.For<IMapper<Author, AuthorDto>>();
            authorMapper.ToDomain(Arg.Any<AuthorDto>())
                .Returns(new Author());

            var sellerMapper = Substitute.For<IMapper<Seller, SellerDto>>();
            sellerMapper.ToDomain(Arg.Any<SellerDto>())
                .Returns(new Seller());

            var categoryMapper = Substitute.For<IMapper<ProductCategory, ProductCategoryDto>>();
            categoryMapper.ToDomain(Arg.Any<ProductCategoryDto>())
                .Returns(new ProductCategory());

            return new List<IMapper>
            {
                commandMapper,
                productMapper,
                authorMapper,
                sellerMapper,
                categoryMapper
            };
        }

        [Fact]
        public async Task Handle_ThrowsArgumentNullException_WhenComandNull()
        {
            var productRepository = Substitute.For<IProductRepository>();
            var validator = Substitute.For<IValidator<UpdateProductCommand>>();
            var mappers = GetMappers();
            var handler = new UpdateProductHandler(productRepository, validator, mappers);
            Func<Task> act = async () => await handler.Handle(null!, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("command");
        }

        [Fact]
        public async Task Handle_CallsValidationRules_WhenCommandIsNotNull()
        {
            var productRepository = Substitute.For<IProductRepository>();
            var validator = Substitute.For<IValidator<UpdateProductCommand>>();

            validator.ValidateAsync(
                Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>()).
                Returns(Task.FromResult(new FluentValidation.Results.ValidationResult()));

            var mappers = GetMappers();
            var handler = new UpdateProductHandler(productRepository, validator, mappers);

            var command = new UpdateProductCommand()
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description",
                Author = new AuthorDto() { Id = 1 },
                Seller = new SellerDto() { Id = 1 },
                ChildProducts = [new() { Id = 2 }],
                Category = new ProductCategoryDto() { Id = 1 },
                IsActive = true,
                Price = 9.99m
            };
            await handler.Handle(command, CancellationToken.None);

            await validator.Received(1).ValidateAsync(
            Arg.Is<ValidationContext<UpdateProductCommand>>(ctx =>
            ctx.InstanceToValidate == command),
            Arg.Any<CancellationToken>());
        }
        [Fact]
        public async Task Handle_UpdatesProduct_WhenCommandIsValid()
        {
            var productRepository = Substitute.For<IProductRepository>();
            var validator = Substitute.For<IValidator<UpdateProductCommand>>();
            validator.ValidateAsync(
                Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>()).
                Returns(Task.FromResult(new FluentValidation.Results.ValidationResult()));
            var mappers = GetMappers();
            var handler = new UpdateProductHandler(productRepository, validator, mappers);
            var command = new UpdateProductCommand()
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description",
                Author = new AuthorDto() { Id = 1 },
                Seller = new SellerDto() { Id = 1 },
                ChildProducts = [new() { Id = 2 }],
                Category = new ProductCategoryDto() { Id = 1 },
                IsActive = true,
                Price = 9.99m
            };
            await handler.Handle(command, CancellationToken.None);
            await productRepository.Received(1).SaveProductAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        }


        [Fact]
        public async Task Handle_CallsSaveProductAsync_WhenCommandIsValid()
        {
            var productRepository = Substitute.For<IProductRepository>();
            var validator = Substitute.For<IValidator<UpdateProductCommand>>();
            validator.ValidateAsync(
                Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>()).
                Returns(Task.FromResult(new FluentValidation.Results.ValidationResult()));

            var mappers = GetMappers();
            var handler = new UpdateProductHandler(productRepository, validator, mappers);

            var command = new UpdateProductCommand()
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description",
                Author = new AuthorDto() { Id = 1 },
                Seller = new SellerDto() { Id = 1 },
                ChildProducts = [new() { Id = 2 }],
                Category = new ProductCategoryDto() { Id = 1 },
                IsActive = true,
                Price = 9.99m
            };

            await handler.Handle(command, CancellationToken.None);

            await productRepository.Received(1).SaveProductAsync(Arg.Is<Product>(o => o.Id == command.Id),
                Arg.Any<CancellationToken>());
        }
    }
}

