using FluentValidation;
using FluentValidation.Results;
using LearnStore.Application.Commands.ProductCommands;
using LearnStore.Application.UseCases;
using NSubstitute.ExceptionExtensions;
using System;

namespace LearnStore.Tests.Unit.Application.UseCases
{
    public class CreateProductHandlerTests
    {
        private readonly Fixture _fixture = new();
        private List<IMapper> GetMappers()
        {
            var commandMapper = Substitute.For<IMapper<Product, CreateProductCommand>>();

            commandMapper.ToDomain(Arg.Any<CreateProductCommand>())
                .Returns(new Product());

            var productMapper = Substitute.For<IMapper<Product, ProductDto>>();
            productMapper.ToDomain(Arg.Any<ProductDto>())
                .Returns(call =>
                {
                    var dto = call.Arg<ProductDto>();
                    return new Product { Id = dto.Id };
                });

            var authorMapper = Substitute.For<IMapper<Author, AuthorDto>>();
            authorMapper.ToDomain(Arg.Any<AuthorDto>())
                .Returns(call =>
                {
                    var dto = call.Arg<AuthorDto>();
                    return new Author { Id = dto.Id };
                });

            var sellerMapper = Substitute.For<IMapper<Seller, SellerDto>>();
            sellerMapper.ToDomain(Arg.Any<SellerDto>())
                .Returns(call =>
                {
                    var dto = call.Arg<SellerDto>();
                    return new Seller { Id = dto.Id };
                });

            var categoryMapper = Substitute.For<IMapper<ProductCategory, ProductCategoryDto>>();
            categoryMapper.ToDomain(Arg.Any<ProductCategoryDto>())
                .Returns(call =>
                {
                    var dto = call.Arg<ProductCategoryDto>();
                    return new ProductCategory { Id = dto.Id };
                });

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
        public async Task Handle_WhenComandNull_ThrowsArgumentNullException()
        {
            var productRepository = Substitute.For<IProductRepository>();
            var validator = Substitute.For<IValidator<CreateProductCommand>>();
            var mappers = GetMappers();

            var handler = new CreateProductHandler(productRepository, validator, mappers);

            Func<Task> act = async () => await handler.Handle(null!, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("command");
        }

        [Fact]
        public async Task Handle_WhenCommandIsNotNull_CallsValidationRules()
        {
            var productRepository = Substitute.For<IProductRepository>();
            var validator = Substitute.For<IValidator<CreateProductCommand>>();
            validator.ValidateAsync(
            Arg.Any<IValidationContext>(),
            Arg.Any<CancellationToken>())
    .       Returns(Task.FromResult(new FluentValidation.Results.ValidationResult()));

            var mappers = GetMappers();
            var handler = new CreateProductHandler(productRepository, validator, mappers);
            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.0m,
                Author = new() { Id = 1 },
                Seller = new() { Id = "1" },
                Category = new() { Id = 1 },
                ChildProducts = [new() { Id = 1 }]
            };
            await handler.Handle(command, CancellationToken.None);
            await validator.Received(1).ValidateAsync(
            Arg.Is<ValidationContext<CreateProductCommand>>(ctx =>
                ctx.InstanceToValidate == command),
                Arg.Any<CancellationToken>());


        }

        [Fact]
        public async Task Handle_WhenCommandIsValid_SavesProduct()
        {
            var productRepository = Substitute.For<IProductRepository>();

            var validator = Substitute.For<IValidator<CreateProductCommand>>();
            validator.ValidateAsync(
                Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new FluentValidation.Results.ValidationResult()));

            var mappers = GetMappers();

            var handler = new CreateProductHandler(productRepository, validator, mappers);

            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.0m,
                Author = new() { Id = 1 },
                Seller = new() { Id = "1" },
                Category = new() { Id = 1 },
                ChildProducts = [new() { Id = 1 }]
            };

            Product? savedProduct = null;

            productRepository
                .When(x => x.SaveProductAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>()))
                .Do(x => savedProduct = x.Arg<Product>());

            await handler.Handle(command, CancellationToken.None);

            await productRepository.Received(1)
                .SaveProductAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());

            savedProduct.Should().NotBeNull();
            savedProduct!.Name.Should().Be(command.Name);
            savedProduct.Description.Should().Be(command.Description);
            savedProduct.Price.Should().Be(command.Price);
            savedProduct!.Author.Id.Should().Be(command.Author.Id);
            savedProduct!.Seller.Id.Should().Be(command.Seller.Id);
            savedProduct!.Category.Id.Should().Be(command.Category.Id);
            savedProduct!.ChildProducts.Count.Should().Be(command.ChildProducts.Count);
        }

        [Fact]
        public async Task Handle_WhenValidatorThrowsException_ThrowsValidationException()
        {
            var productRepository = Substitute.For<IProductRepository>();
            var validator = Substitute.For<IValidator<CreateProductCommand>>();

            var validationFailures = new List<FluentValidation.Results.ValidationFailure>
            {
                new("Name", "Name is required"),
            };
            var validationResult = new FluentValidation.Results.ValidationResult(validationFailures);
            validator.ValidateAsync(
                Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>())
                .ThrowsAsync(new FluentValidation.ValidationException(validationFailures));

            var mappers = GetMappers();
            var handler = new CreateProductHandler(productRepository, validator, mappers);
            var command = new CreateProductCommand
            {
                Description = "Test Description",
                Price = 10.0m,
                Author = new() { Id = 1 },
                Seller = new() { Id = "1" },
                Category = new() { Id = 1 },
                ChildProducts = [new() { Id = 1 }]
            };
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
            await act.Should().ThrowAsync<FluentValidation.ValidationException>().WithMessage("*Name is required*");
        }
    }
}
