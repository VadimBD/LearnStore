using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Infrastructure.DataAccess.MsSql
{
    public class EFAuthorRepositoryTests
    {
        [Fact]
        public async Task SaveSellerAsync_WhenNullSeller_ThrowArgumentNullException()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);


            var repository = new EFAuthorRepository(context);
            Func<Task> act = async () => await repository.SaveAuthorAsync(null!, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("author");
        }

        [Fact]

        public async Task SaveSellerAsync_WhenSellerIdMatch_UpdatesExistingAuthor()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);

            var author = new Author { Id = 1, FirstName = "John", LastName = "Doe", MiddleName = "Jane", Info = "Test Info" };
            var author2 = new Author { Id = 2, FirstName = "Jane", LastName = "Smith", MiddleName = "John", Info = "Test Info" };
            context.Authors.Add(author);
            context.Authors.Add(author2);
            context.SaveChanges();

            var context2 = new AppDbContext(options);
            var repository = new EFAuthorRepository(context2);
            var updatedAuthor = new Author { Id = 1, FirstName = "Updated John", LastName = "Updated Doe", MiddleName = "Updated Jane", Info = "Updated Test Info" };
            await repository.SaveAuthorAsync(updatedAuthor, CancellationToken.None);

            var savedAuthor = await context2.Authors.FindAsync(1);
            savedAuthor.Should().NotBeNull();
            savedAuthor!.FirstName.Should().Be("Updated John");
            savedAuthor!.MiddleName.Should().Be("Updated Jane");
            savedAuthor!.LastName.Should().Be("Updated Doe");
            savedAuthor!.Info.Should().Be("Updated Test Info");
        }

        [Fact]
        public async Task SaveAuthorAsync_WhenAuthorIdIsZero_AddsNewAuthor()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var author = new Author { FirstName = "New Author", LastName = "New Last Name", MiddleName = "New Middle Name", Info = "New Test Info" };
            context.Authors.Add(author);
            context.SaveChanges();
            var repository = new EFAuthorRepository(context);
            await repository.SaveAuthorAsync(author, CancellationToken.None);

            var savedAuthor = await context.Authors.FirstOrDefaultAsync(a => a.FirstName == "New Author");
            savedAuthor.Should().NotBeNull();
            savedAuthor.FirstName.Should().Be("New Author");
        }

        [Fact]
        public async Task SaveAuthorAsync_WhenFirstNameIsNull_ThrowsArgumentNullException()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var context = new AppDbContext(options);
            var repository = new EFAuthorRepository(context);

            var author = new Author { LastName = "Doe", Info = "Test Info" };

            Func<Task> act = () => repository.SaveAuthorAsync(author, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentException>().WithParameterName("FirstName");
        }
        [Fact]
        public async Task SaveAuthorAsync_WhenLastNameIsNull_ThrowsArgumentNullException()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var repository = new EFAuthorRepository(context);
            var author = new Author { Id = 1, FirstName = "John", Info = "Test Info" };
            Func<Task> act = () => repository.SaveAuthorAsync(author, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentException>().WithParameterName("LastName");

        }
        [Fact]
        public async Task SaveAuthorAsync_WhenInfoIsNull_ThrowsArgumentNullException()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var repository = new EFAuthorRepository(context);
            var author = new Author { Id = 1, FirstName = "John", LastName = "Doe", MiddleName = "Jane" };
            Func<Task> act = () => repository.SaveAuthorAsync(author, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentException>().WithParameterName("Info");
        }


        [Fact]
        public async Task GetAuthorAsync_WhenAuthorIdIsValid_ReturnsAuthor()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);

            var author = new Author
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Info = "Test Info"
            };

            var author2 = new Author
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                Info = "Test Info 2"
            };

            context.Authors.Add(author);
            context.Authors.Add(author2);
            context.SaveChanges();

            var repository = new EFAuthorRepository(context);

            var criteria = new AuthorSearchCriteria
            {
                AuthorId = 1
            };

            var result = await repository.GetAuthorsAsync(criteria, CancellationToken.None);

            result.Should().NotBeNull();

            result.Should().Contain(a =>
                a.Id == author.Id &&
                a.FirstName == "John" &&
                a.LastName == "Doe");
        }

        [Fact]
        public async Task GetAuthorsAsync_WhenAuthorIdIsInvalid_ReturnsEmpty()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var author = new Author { Id = 1, FirstName = "John", LastName = "Doe", Info = "Test Info" };

            context.Authors.Add(author);
            context.SaveChanges();
            var repository = new EFAuthorRepository(context);
            var criteria = new AuthorSearchCriteria {AuthorId = 999 };

            var result = await repository.GetAuthorsAsync(criteria, CancellationToken.None);

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAuthorsAsync_WhenAuthorsCriteriaIsNull_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var repository = new EFAuthorRepository(context);
            Func<Task> act = () => repository.GetAuthorsAsync(null!, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("criteria");
        }

        [Fact]
        public async Task GetAuthorsAsync_WhenCriteriaIsEmpty_ReturnsAllAuthors()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var author1 = new Author { Id = 1, FirstName = "John", LastName = "Doe", Info = "Test Info 1" };
            var author2 = new Author { Id = 2, FirstName = "Jane", LastName = "Smith", Info = "Test Info 2" };
            context.Authors.Add(author1);
            context.Authors.Add(author2);
            context.SaveChanges();
            var repository = new EFAuthorRepository(context);
            var criteria = new AuthorSearchCriteria();
            var result = await repository.GetAuthorsAsync(criteria, CancellationToken.None);
            result.Should().NotBeNull();
            result.Should().Contain(author1);
            result.Should().Contain(author2);
        }
        [Fact]
        public async Task GetAuthorsAsync_WhenCriteriaValid_ReturnsAuthors()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var author1 = new Author { Id = 1, FirstName = "John", LastName = "Doe", Info = "Test Info 1" };
            var author2 = new Author { Id = 2, FirstName = "Jane", LastName = "Smith", Info = "Test Info 2" };

            context.Authors.Add(author1);
            context.Authors.Add(author2);
            context.SaveChanges();
            var repository = new EFAuthorRepository(context);
            var criteria = new AuthorSearchCriteria { FirstName = "John" };
            var result = await repository.GetAuthorsAsync(criteria, CancellationToken.None);
            result.Should().NotBeNull();
            result.Should().Contain(author1);
        }
    }
}
