using LearnStore.Application.Commands.OrderCommands;
using LearnStore.Application.DTO;
using LearnStore.Application.Queries;
using LearnStore.Domain.Entities;
using LearnStore.Web.MVC.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace LearnStore.Web.MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateOrder()
        {
            var orderViewModel = new OrderViewModel();
            return View(orderViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateOrder(OrderViewModel orderViewModel)
        {
            if(!ModelState.IsValid)
                return View(orderViewModel);
            var id = GetCurrentUserId();

            var query = new CreateOrderCommand()
            {
                Customer = new CustomerDto() { Id = id },
                Items = orderViewModel.Items.Select(i => new OrderItemDto()
                {
                    Product = new ProductDto() { Id = i.Id },
                    Quantity = i.Quantity
                }).ToList()
            };
            await _mediator.Send(query);
            return RedirectToAction("Orders");
        }
       
        public string GetCurrentUserId()
        {
            return User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? string.Empty;
        }
    }
}
