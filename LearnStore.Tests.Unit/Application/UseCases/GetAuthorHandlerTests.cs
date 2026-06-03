using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.UseCases
{
    public class GetAuthorHandlerTests
    {

        [Fact]
        public async Task Handle_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var productRepository = Substitute.For<IAuthorRepository>();
            var productMapper = Substitute.For<IMapper<Author, AuthorDto>>();
            var handler = new GetAuthorHandler(productRepository, productMapper);
            // Act
            Func<Task> act = async () => await handler.Handle(null!, CancellationToken.None);
            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("request");
        }
        [Fact]
        public async Task Handle_WhenProductIdLessThanOne_ThrowsArgumentException()
        {

            // Arrange
            var productRepository = Substitute.For<IAuthorRepository>();
            var productMapper = Substitute.For<IMapper<Author, AuthorDto>>();
            var handler = new GetAuthorHandler(productRepository, productMapper);
            var query = new GetAuthorQuery() { AuthorId = 0 };
            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);
            // Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>().WithParameterName("AuthorId").WithMessage("AuthorId must be greater than zero.*");

        }
        [Fact]
        public async Task Handle_WhenQueryIsValid_ReturnsAuthor()
        {
            // Arrange
            var authorRepository = Substitute.For<IAuthorRepository>();

            var author = new Author{ Id = 1, FirstName = "Author1",LastName = "LastName1", MiddleName = "MiddleName1" };
            authorRepository .GetAuthorsAsync(Arg.Any<AuthorSearchCriteria>(), Arg.Any<CancellationToken>()).Returns(new[] { author });
            var mapper = Substitute.For<IMapper<Author, AuthorDto>>();
            mapper.ToDto(author).Returns(new AuthorDto
            {
                Id = 1,
                FirstName = "Author1",
                LastName = "LastName1",
                MiddleName = "MiddleName1"
            });
            var handler = new GetAuthorHandler(authorRepository, mapper);
            var query = new GetAuthorQuery { AuthorId = 1};
            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.FirstName.Should().Be("Author1");

            await authorRepository.Received(1) .GetAuthorsAsync(Arg.Is<AuthorSearchCriteria>(c => c.AuthorId == 1),Arg.Any<CancellationToken>());
            mapper.Received(1).ToDto(Arg.Is<Author>(a =>a.Id == 1 &&a.FirstName == "Author1" && a.LastName == "LastName1" && a.MiddleName == "MiddleName1"));
        }
    }
}

