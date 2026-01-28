using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Mappers
{
    public class ProductCategoryMapperTests
    {
        
        private readonly Fixture _fixture = new();
        [Fact]
        public void ToDto_WhenProductCategoryIsNull_ThrowsArgumentNullException()
        {
            var mapper = new ProductCategoryMapper();
            // Act
            Action action = () => mapper.ToDto(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("productCategory");
        }

        [Fact]
        public void ToDto_WhenProductCategoryIsValid_ReturnsExpectedDto()
        {
            var mapper = new ProductCategoryMapper();
            // Arrange
            var productCategory = _fixture.Create<ProductCategory>();
            var expectedDto = new ProductCategoryDto
            {
                Id = productCategory.Id,
                Name = productCategory.Name,
            };
            // Act
            var result = mapper.ToDto(productCategory);
            // Assert
            result.Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public void ToDomain_WhenProductCategoryDtoIsNull_ThrowsArgumentNullException()
        {
            var mapper = new ProductCategoryMapper();
            // Act
            Action action = () => mapper.ToDomain(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("productCategoryDto");
        }
        [Fact]
        public void ToDomain_WhenProductCategoryDtoIsValid_ReturnsexpectedDomain()
        {
            var mapper = new ProductCategoryMapper();
            // Arrange
            var productCategoryDto = _fixture.Create<ProductCategoryDto>();
            var expectedDomain = new ProductCategory
            {
                Id = productCategoryDto.Id,
                Name = productCategoryDto.Name,
            };
            // Act
            var result = mapper.ToDomain(productCategoryDto);
            // Assert
            result.Should().BeEquivalentTo(expectedDomain);
        }
    }
}
