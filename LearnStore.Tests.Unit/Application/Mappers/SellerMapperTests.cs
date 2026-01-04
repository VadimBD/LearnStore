using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Mappers
{
    public class SellerMapperTests
    {
        private readonly Faker _faker = new();
        private readonly Fixture _fixture = new();

        [Fact]
        public void ToDto_WhenSellerIsNull_ThrowsArgumentNullException()
        {
            var mapper = new SellerMapper();
            // Act
            Action action = () => mapper.ToDto(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("seller");
        }

        [Fact]
        public void ToDto_WhenSellerIsValid_ReturnsExpectedDto()
        {
            var mapper = new SellerMapper();
            // Arrange
            var seller = _fixture.Create<Seller>();
            var expectedDto = new SellerDto
            {
                Id = seller.Id,
                Name = seller.Name,
                EmailAddress = seller.EmailAddress,
                PhoneNumber = seller.PhoneNumber
            };
            // Act
            var result = mapper.ToDto(seller);
            // Assert
            result.Should().BeEquivalentTo(expectedDto);
        }
        [Fact]
        public void ToEntity_WhenSellerDtoIsNull_ThrowsArgumentNullException()
        {
            var mapper = new SellerMapper();
            // Act
            Action action = () => mapper.ToEntity(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("sellerDto");
        }
        [Fact]
        public void ToEntity_WhenSellerDtoIsValid_ReturnsExpectedEntity()
        {
            var mapper = new SellerMapper();
            // Arrange
            var sellerDto = _fixture.Create<SellerDto>();
            var expectedEntity = new Seller
            {
                Id = sellerDto.Id,
                Name = sellerDto.Name,
                EmailAddress = sellerDto.EmailAddress,
                PhoneNumber = sellerDto.PhoneNumber
            };
            // Act
            var result = mapper.ToEntity(sellerDto);
            // Assert
            result.Should().BeEquivalentTo(expectedEntity);
        }
    }
}
