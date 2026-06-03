using LearnStore.Admin.Models;
using LearnStore.Application.Commands.OrderCommands;
using LearnStore.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnStore.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index([FromQuery] OrdersFilter filter)
        {
            var query = new GetOrdersQuery()
            {
                CustomerId = filter.CustomerId,
            };
            var orders = await _mediator.Send(query);
            return View(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var query = new GetOrderQuery(id);
            var order = await _mediator.Send(query);
            if (order == null)
                return NotFound();

            var productsQuery = new GetProductsQuery();
            var products = await _mediator.Send(productsQuery);

            var orderVM = new OrderViewModel()
            {
                Order = order,
                Products=products.Where(p=>p.IsActive||order.Items.Any(i => i.Product?.Id == p.Id)).ToList(),
            };
            return View(orderVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit( OrderViewModel orderVM)
        {
            if (orderVM is null || orderVM.Order is null)
                return BadRequest();
            if (!ModelState.IsValid)
                return View(orderVM);
            var command = new UpdateOrderCommand()
            {
                Id = orderVM.Order.Id,
                Customer = new() { Id = orderVM.Order.Customer.Id},
                Items = orderVM.Order.Items,
            };
            await _mediator.Send(command);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id) 
        {
            var command = new DeleteOrderCommand(id);
            await _mediator.Send(command);

            return RedirectToAction("Index");
        }
    }
}
