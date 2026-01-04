using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Mappers
{
    public class AuthorMapperTests
    {
        private readonly Faker _faker = new();
        private readonly Fixture _fixture = new();
        [Fact]
        public void ToDto_WhenAuthorIsNull_ThrowsArgumentNullException()
        {
            var mapper = new AuthorMapper();
            // Act
            Action action = () => mapper.ToDto(null);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("author");
        }
        [Fact]
        public void ToDto_WhenAuthorIsValid_ReturnsExpectedDto()
        {
            var mapper = new AuthorMapper();
            // Arrange
            var author = _fixture.Create<Author>();
            var expectedDto = new AuthorDto
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                MiddleName = author.MiddleName,
                Info = author.Info
            };
            // Act
            var result = mapper.ToDto(author);
            // Assert
            result.Should().BeEquivalentTo(expectedDto);

        }
        [Fact]
        public void ToEntity_WhenAuthorDtoIsNull_ThrowsArgumentNullException()
        {
            var mapper = new AuthorMapper();
            // Act
            Action action = () => mapper.ToEntity(null);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("authorDto");
        }
        [Fact]
        public void ToEntity_WhenAuthorDtoIsValid_ReturnsExpectedEntity()
        {
            var mapper = new AuthorMapper();
            // Arrange
            var authorDto = _fixture.Create<AuthorDto>();
            var expectedEntity = new Author
            {
                Id = authorDto.Id,
                FirstName = authorDto.FirstName,
                LastName = authorDto.LastName,
                MiddleName = authorDto.MiddleName,
                Info = authorDto.Info
            };
            // Act
            var result = mapper.ToEntity(authorDto);
            // Assert
            result.Should().BeEquivalentTo(expectedEntity);
        }
    }
}
