using LearnStore.Admin.Models;
using LearnStore.Application.Commands.AuthorCommands;
using LearnStore.Application.Commands.SellerCommands;
using LearnStore.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnStore.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthorController : Controller
    {
        private readonly IMediator _mediator;

        public AuthorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index([FromQuery] AuthorsFilter filter)
        {
            var query = new GetAuthorsQuery()
            {
                FirstName = filter.FirstName,
                LastName = filter.LastName,
                MiddleName = filter.MiddleName
            };
            var authors = await _mediator.Send(query);
            return View(authors);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AuthorViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var command = new CreateAuthorCommand()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName
            };

            await _mediator.Send(command);
            return RedirectToAction("Index");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var query = new GetAuthorQuery { AuthorId = id };
            var author = await _mediator.Send(query);
            if (author == null)
                return NotFound();
            var model = new AuthorViewModel()
            {
                AuthorId = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                MiddleName = author.MiddleName
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AuthorViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var query = new GetAuthorQuery { AuthorId = model.AuthorId };
            var author = await _mediator.Send(query);
            if (author == null)
                return NotFound();

            var command = new UpdateAuthorCommand
            {
                Id = model.AuthorId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName
            };
            await _mediator.Send(command);
            return RedirectToAction("Index");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteAuthorCommand (id);
            await _mediator.Send(command);
            return RedirectToAction("Index");
        }
    }
}
