using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.UseCases
{
 
    public class GetSellerHandlerTests
    {
        [Fact]
        public async Task Handle_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var sellerRepository = Substitute.For<ISellerRepository>();
            var sellerMapper = Substitute.For<IMapper<Seller, SellerDto>>();

            var handler = new GetSellerHandler(sellerRepository, sellerMapper);

            // Act
            Func<Task> act = async () =>
                await handler.Handle(null!, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentNullException>()
                .WithParameterName("request");
        }

        [Fact]
        public async Task Handle_WhenSellerIdIsEmpty_ThrowsArgumentException()
        {
            // Arrange
            var sellerRepository = Substitute.For<ISellerRepository>();
            var sellerMapper = Substitute.For<IMapper<Seller, SellerDto>>();

            var handler = new GetSellerHandler(sellerRepository, sellerMapper);

            var query = new GetSellerQuery(string.Empty);

            // Act
            Func<Task> act = async () =>
                await handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithParameterName("Id");
        }

        [Fact]
        public async Task Handle_WhenSellerExists_ReturnsSellerDto()
        {
            // Arrange
            var sellerRepository = Substitute.For<ISellerRepository>();
            var sellerMapper = Substitute.For<IMapper<Seller, SellerDto>>();

            var seller = new Seller
            {
                Id = "1",
                Name = "Vadim"
            };

            var sellerDto = new SellerDto
            {
                Id = "1",
                Name = "Vadim"
            };

            var query = new GetSellerQuery("1");

            sellerRepository
                .GetSellersAsync(
                    Arg.Any<SellerSearchCriteria>(),
                    Arg.Any<CancellationToken>())
                .Returns(new List<Seller> { seller });

            sellerMapper
                .ToDto(seller)
                .Returns(sellerDto);

            var handler = new GetSellerHandler(sellerRepository, sellerMapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(sellerDto);

            await sellerRepository.Received(1)
                .GetSellersAsync(
                    Arg.Is<SellerSearchCriteria>(c => c.Id == "1"),
                    Arg.Any<CancellationToken>());

            sellerMapper.Received(1)
                .ToDto(seller);
        }

        [Fact]
        public async Task Handle_WhenSellerNotFound_ReturnsNull()
        {
            // Arrange
            var sellerRepository = Substitute.For<ISellerRepository>();
            var sellerMapper = Substitute.For<IMapper<Seller, SellerDto>>();

            sellerRepository
                .GetSellersAsync(
                    Arg.Any<SellerSearchCriteria>(),
                    Arg.Any<CancellationToken>())
                .Returns(new List<Seller>());

            var handler = new GetSellerHandler(sellerRepository, sellerMapper);

            var query = new GetSellerQuery("1");

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();

            sellerMapper.DidNotReceive()
                .ToDto(Arg.Any<Seller>());
        }

        [Fact]
        public async Task Handle_PassesCorrectCancellationToken()
        {
            // Arrange
            var sellerRepository = Substitute.For<ISellerRepository>();
            var sellerMapper = Substitute.For<IMapper<Seller, SellerDto>>();

            var token = new CancellationTokenSource().Token;

            sellerRepository
                .GetSellersAsync(
                    Arg.Any<SellerSearchCriteria>(),
                    token)
                .Returns(new List<Seller> { new Seller() });

            sellerMapper
                .ToDto(Arg.Any<Seller>())
                .Returns(new SellerDto());

            var handler = new GetSellerHandler(sellerRepository, sellerMapper);

            // Act
            await handler.Handle(new GetSellerQuery("1"), token);

            // Assert
            await sellerRepository.Received(1)
                .GetSellersAsync(
                    Arg.Any<SellerSearchCriteria>(),
                    token);
        }

        [Fact]
        public async Task Handle_WhenSellerNotFound_DoesNotCallMapper()
        {
            // Arrange
            var sellerRepository = Substitute.For<ISellerRepository>();
            var sellerMapper = Substitute.For<IMapper<Seller, SellerDto>>();

            sellerRepository
                .GetSellersAsync(
                    Arg.Any<SellerSearchCriteria>(),
                    Arg.Any<CancellationToken>())
                .Returns(new List<Seller>());

            var handler = new GetSellerHandler(sellerRepository, sellerMapper);

            // Act
            await handler.Handle(new GetSellerQuery("1"), CancellationToken.None);

            // Assert
            sellerMapper.DidNotReceive()
                .ToDto(Arg.Any<Seller>());
        }
    }
}
