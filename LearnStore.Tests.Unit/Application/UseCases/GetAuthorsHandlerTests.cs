using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.UseCases
{
    public class GetAuthorsHandlerTests
    {
        public IMapper<Author, AuthorDto> GetAuthorMapper()
        {
            var authorMapper = Substitute.For<IMapper<Author, AuthorDto>>();
            authorMapper
                .ToDto(Arg.Any<Author>())
            .Returns(call =>
            {
                var a = call.Arg<Author>();
                return new AuthorDto
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    MiddleName = a.MiddleName
                };
            });
            return authorMapper;
        }

    

        [Fact]
        public async Task Handle_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            var authorRepository = Substitute.For<IAuthorRepository>();
            var authorMapper = GetAuthorMapper();
            var handler = new GetAuthorsHandler(authorRepository, authorMapper);

            Func<Task> act = async () => await handler.Handle(null!, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("request");
        }
        [Fact]
        public async Task Handle_WhenAuthorIdMatches_ReturnsAuthors()
        {
            var authorRepository = Substitute.For<IAuthorRepository>();

            var authorId = 1;

            authorRepository.Authors.Returns(
                new List<Author>
                {
                    new Author { Id = 1, FirstName = "Author1", LastName = "LastName1", MiddleName = "MiddleName1" },
                    new Author { Id = 2, FirstName = "Author2", LastName = "LastName2", MiddleName = "MiddleName2" },
                    new Author { Id = 3, FirstName = "Author3", LastName = "LastName3", MiddleName = "MiddleName3" },
                    new Author { Id = 4, FirstName = "Author4", LastName = "LastName4", MiddleName = "MiddleName4" }
                }.AsQueryable());

            var authorMapper = GetAuthorMapper();
            var handler = new GetAuthorsHandler(authorRepository, authorMapper);
            var query = new GetAuthorsQuery { AuthorId = authorId };

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().HaveCount(1);
            result.All(p => p.Id == authorId).Should().BeTrue();
        }

        [Fact]
        public async Task Handle_WhenNameContainsFilter_ReturnsAuthors()
        {
            var authorRepository = Substitute.For<IAuthorRepository>();
            authorRepository.Authors.Returns([
                new Author { Id = 1, FirstName = "Author1", LastName = "LastName1", MiddleName = "MiddleName1" },
                new Author { Id = 2, FirstName = "Author2", LastName = "LastName2", MiddleName = "MiddleName2" },
                new Author { Id = 3, FirstName = "Author3", LastName = "LastName3", MiddleName = "MiddleName3" },
                new Author { Id = 4, FirstName = "Author4", LastName = "LastName4", MiddleName = "MiddleName4" }
                ]);

            var authorMapper = GetAuthorMapper();
            var handler = new GetAuthorsHandler(authorRepository, authorMapper);

            var query = new GetAuthorsQuery
            {
                FirstName = "Author"
            };

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().HaveCount(4);
            result.All(p => p.FirstName.Contains("Author")).Should().BeTrue();
        }



        [Fact]
        public async Task Handle_WhenNoFiltersApplied_ReturnsAllAuthors()
        {
            var authorRepository = Substitute.For<IAuthorRepository>();
            authorRepository.Authors.Returns([
                new Author { Id = 1, FirstName = "Author1", LastName = "LastName1", MiddleName = "MiddleName1" },
                new Author { Id = 2, FirstName = "Author2", LastName = "LastName2", MiddleName = "MiddleName2" },
                new Author { Id = 3, FirstName = "Author3", LastName = "LastName3", MiddleName = "MiddleName3" },
                new Author { Id = 4, FirstName = "Author4", LastName = "LastName4", MiddleName = "MiddleName4" }
                ]);
            var authorMapper = GetAuthorMapper();
            var handler = new GetAuthorsHandler(authorRepository, authorMapper);
            var query = new GetAuthorsQuery { };
            var result = await handler.Handle(query, CancellationToken.None);
            result.Should().HaveCount(4);
        }


        [Fact]
        public async Task Handle_WhenAllFiltersApplied_ReturnsAuthors()
        {
            var authorRepository = Substitute.For<IAuthorRepository>();
            authorRepository.Authors.Returns([
                new Author { Id = 1, FirstName = "Author1", LastName = "LastName1", MiddleName = "MiddleName1" },
                new Author { Id = 2, FirstName = "Author1", LastName = "LastName1", MiddleName = "MiddleName1" },
                new Author { Id = 3, FirstName = "Author3", LastName = "LastName3", MiddleName = "MiddleName3" },
                new Author { Id = 4, FirstName = "Author4", LastName = "LastName4", MiddleName = "MiddleName4" }
                ]);
            var authorMapper = GetAuthorMapper();
            var handler = new GetAuthorsHandler(authorRepository, authorMapper);
            var query = new GetAuthorsQuery { FirstName = "Author1" , LastName = "LastName1" ,  MiddleName = "MiddleName1" };
            var result = await handler.Handle(query, CancellationToken.None);
            result.Should().HaveCount(2);
        }
          
    }
}
