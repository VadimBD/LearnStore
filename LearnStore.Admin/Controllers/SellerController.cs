using LearnStore.Admin.Models;
using LearnStore.Application.Commands.SellerCommands;
using LearnStore.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnStore.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SellerController : Controller
    {
        private readonly IMediator _mediator;
        public SellerController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] SellersFilter filter)
        {
            var query = new GetSellersQuery
            {
                Name = filter.Name
            };
            var sellers = await _mediator.Send(query);
            return View(sellers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SellerViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var command = new CreateSellerCommand
            {
                Name = model.Name,
                EmailAddress = model.Email,
                PhoneNumber = model.PhoneNumber,
                AccountBalance = 0
            };

            await _mediator.Send(command);

            return RedirectToAction("Index");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var query = new GetSellerQuery(id);
            var seller = await _mediator.Send(query);
            if (seller == null)
                return NotFound();
            var model = new SellerViewModel() 
            {
                Id = seller.Id,
                Name = seller.Name,
                Email = seller.EmailAddress,
                PhoneNumber = seller.PhoneNumber
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SellerViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var query = new GetSellerQuery(model.Id);
            var seller = await _mediator.Send(query);
            if (seller == null)
                return NotFound();

            var command = new UpdateSellerCommand
            {
                Id = model.Id,
                Name = model.Name,
                EmailAddress = model.Email,
                PhoneNumber = model.PhoneNumber,
                AccountBalance = seller.AccountBalance
            };
            await _mediator.Send(command);
            return RedirectToAction("Index");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var command = new DeleteSellerCommand(id);
            await _mediator.Send(command);
            return RedirectToAction("Index");
        }
    }
}
