using LearnStore.Application.Commands.AuthorCommands;
using LearnStore.Application.Commands.ProductCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.UseCases
{
    public class UpdateAuthorHandlerTests
    {
       

        [Fact]
        public async Task Handle_WhenComandNull_ThrowsArgumentNullException()
        {
            var authorRepository = Substitute.For<IAuthorRepository>();
            var validator = Substitute.For<IValidator<UpdateAuthorCommand>>();
          
            var handler = new UpdateAuthorHandler(authorRepository, validator);
            Func<Task> act = async () => await handler.Handle(null!, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("command");
        }

        [Fact]
        public async Task Handle_WhenCommandIsNotNull_CallsValidationRules()
        {
            var authorRepository = Substitute.For<IAuthorRepository>();
            var validator = Substitute.For<IValidator<UpdateAuthorCommand>>();

            validator.ValidateAsync(
                Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>()).
                Returns(Task.FromResult(new FluentValidation.Results.ValidationResult()));

            var handler = new UpdateAuthorHandler(authorRepository, validator);

            var command = new UpdateAuthorCommand()
            {
                Id = 1,
                FirstName = "Test FirstName",
                LastName = "Test LastName",
                MiddleName = "Test MiddleName",
                Info = "Test Info"
            };
            await handler.Handle(command, CancellationToken.None);

            await validator.Received(1).ValidateAsync(
            Arg.Is<ValidationContext<UpdateAuthorCommand>>(ctx =>
            ctx.InstanceToValidate == command),
            Arg.Any<CancellationToken>());
        }
        [Fact]
        public async Task Handle_WhenCommandIsValid_UpdatesAuthor()
        {
            var authorRepository = Substitute.For<IAuthorRepository>();
            var validator = Substitute.For<IValidator<UpdateAuthorCommand>>();
            validator.ValidateAsync(
                Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>()).
                Returns(Task.FromResult(new FluentValidation.Results.ValidationResult()));
          
            var handler = new UpdateAuthorHandler(authorRepository, validator);
            var command = new UpdateAuthorCommand()
            {
                Id = 1,
                FirstName = "Test FirstName",
                LastName = "Test LastName",
                MiddleName = "Test MiddleName",
                Info = "Test Info"
            };
            await handler.Handle(command, CancellationToken.None);
            await authorRepository.Received(1).SaveAuthorAsync(Arg.Any<Author>(), Arg.Any<CancellationToken>());
        }


        [Fact]
        public async Task Handle_WhenCommandIsValid_CallsSaveAuthorAsync()
        {
            var authorRepository = Substitute.For<IAuthorRepository>();
            var validator = Substitute.For<IValidator<UpdateAuthorCommand>>();
            validator.ValidateAsync(
                Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>()).
                Returns(Task.FromResult(new FluentValidation.Results.ValidationResult()));

          
            var handler = new UpdateAuthorHandler(authorRepository, validator);

            var command = new UpdateAuthorCommand()
            {
                Id = 1,
                FirstName = "Test FirstName",
                LastName = "Test LastName",
                MiddleName = "Test MiddleName",
                Info = "Test Info"
            };

            await handler.Handle(command, CancellationToken.None);

            await authorRepository.Received(1).SaveAuthorAsync(Arg.Is<Author>(o => o.Id == command.Id),
                Arg.Any<CancellationToken>());
        }
    }
}
